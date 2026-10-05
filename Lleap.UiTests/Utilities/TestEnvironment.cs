using System;
using System.Diagnostics;
using System.Security.Principal;



namespace Lleap.UiTests.Utilities
{
    public static class TestEnvironment
    {
        public static bool HasAdministratorRights()
        {
            using var currentIdentity = WindowsIdentity.GetCurrent();
            var currentUser = new WindowsPrincipal(currentIdentity);

            return currentUser.IsInRole(
                WindowsBuiltInRole.Administrator);
        }



        public static Process StartSimulationHome()
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = @"C:\Program Files (x86)\Laerdal Medical\Laerdal Simulation Home\LaunchPortal.exe",
                WorkingDirectory = @"C:\Program Files (x86)\Laerdal Medical\Laerdal Simulation Home",
                UseShellExecute = true
            });

            if (process == null)
            {
                throw new InvalidOperationException("Simulation Home process did not start.");
            }
            return process;
        }
    }
}