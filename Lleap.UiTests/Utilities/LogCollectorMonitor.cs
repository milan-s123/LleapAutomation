using System.Collections.Generic;
using System.Diagnostics;

namespace Lleap.UiTests.Utilities
{
    public class LogCollectorMonitor
    {
        private readonly List<int> existingIds = new();



        public void RememberExistingProcesses()
        {
            existingIds.Clear();

            foreach (var process in Process.GetProcessesByName("LLEAPLogView"))
            {
                using (process)
                {
                    existingIds.Add(process.Id);
                }
            }
        }

        

        public Process? FindNewProcess()
        {
            foreach (var process in Process.GetProcessesByName("LLEAPLogView"))
            {
                if (!existingIds.Contains(process.Id))
                {
                    return process;
                }

                process.Dispose();
            }

            return null;
        }
    }
}