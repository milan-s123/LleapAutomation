using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Lleap.UiTests.Utilities
{
    public class LogArchiveChecker
    {
        public string Folder { get; } = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.CommonDocuments),
            "Laerdal Report Zipped");

        private readonly Dictionary<string, DateTime> existingArchives = new();

        public void RememberExistingArchives()
        {
            existingArchives.Clear();

            if (!Directory.Exists(Folder))
                return;

            foreach (var file in Directory.GetFiles(Folder, "*.zip"))
            {
                existingArchives[file] = File.GetLastWriteTimeUtc(file);
            }
        }




        public string? FindNewArchiveWithClientLog()
        {
            if (!Directory.Exists(Folder))
                return null;

            foreach (var file in Directory.GetFiles(Folder, "*.zip"))
            {
                if (existingArchives.ContainsKey(file) &&
                    existingArchives[file] == File.GetLastWriteTimeUtc(file))
                {
                    continue;
                }

                if (ContainsReadableClientLog(file))
                    return file;
            }
            return null;
        }



        private bool ContainsReadableClientLog(string file)
        {
            try
            {
                using var archive = ZipFile.OpenRead(file);

                foreach (var entry in archive.Entries)
                {
                    if (!entry.Name.StartsWith("Client_"))
                        continue;

                    using var reader = new StreamReader(entry.Open());
                    var content = reader.ReadToEnd();

                    if (!string.IsNullOrWhiteSpace(content))
                        return true;
                }
            }
            catch (InvalidDataException)
            {
                // The ZIP may still be incomplete.
            }
            catch (IOException)
            {
                // The file may still be in use.
            }
            return false;
        }


    }
}