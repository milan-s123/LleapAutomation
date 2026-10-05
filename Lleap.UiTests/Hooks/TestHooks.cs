using System;
using System.IO;
using Reqnroll;
using FlaUI.Core.Capturing;
using FlaUI.UIA3;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Lleap.UiTests.Hooks
{
    [Binding]
    public class TestHooks
    {

        [DllImport("user32.dll")]
        private static extern IntPtr SetThreadDpiAwarenessContext(IntPtr context);


        private string _resultsFolder = "";

        [BeforeScenario]
        public void BeforeScenario()
        {
            _resultsFolder = Path.Combine(
                AppContext.BaseDirectory,
                "TestResults",
                DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff"));

            Directory.CreateDirectory(_resultsFolder);
        }



        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            var step = scenarioContext.StepContext.StepInfo.Text;
            var status = scenarioContext.StepContext.Status;

            var logFile = Path.Combine(_resultsFolder, "steps.txt");

            File.AppendAllText(
                logFile,
                $"{status}: {step}{Environment.NewLine}");



            if (scenarioContext.TestError != null)
            {
                File.AppendAllText(
                    logFile,
                    scenarioContext.TestError.Message + Environment.NewLine);

                try
                {
                    var screenshot = Path.Combine(_resultsFolder, "failure.png");
                    var previousContext = SetThreadDpiAwarenessContext(new IntPtr(-4));

                    if (previousContext == IntPtr.Zero)
                        throw new InvalidOperationException("Could not set screenshot DPI mode.");

                    try
                    {
                        Capture.Screen().ToFile(screenshot);
                    }
                    finally
                    {
                        SetThreadDpiAwarenessContext(previousContext);
                    }
                }

                catch (Exception error)
                {
                    Console.WriteLine("Screenshoot could not be saved: " + error.Message);
                }
            }
        }
        

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            if (!scenarioContext.TryGetValue<Process>(
                "SimulationHomeProcess", out var process))
                return;

            try
            {
                if (!process.HasExited)
                {
                    process.CloseMainWindow();

                    if (!process.WaitForExit(5000))
                    {
                        process.Kill();
                        process.WaitForExit(5000);
                    }
                }
            }
            finally
            {
                process.Dispose();
            }
        }


    }
}