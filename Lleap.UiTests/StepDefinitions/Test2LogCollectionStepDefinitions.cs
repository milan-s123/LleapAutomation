using System;
using Reqnroll;
using FlaUI.UIA3;
using Lleap.UiTests.Pages;
using FlaUI.Core.Tools;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using Lleap.UiTests.Utilities;



namespace Lleap.UiTests.StepDefinitions
{
    [Binding]
    public class Test2LogCollectionStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly LogCollectorMonitor _logCollector = new();
        private readonly LogArchiveChecker _logArchive = new();

        public Test2LogCollectionStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }


        [When(@"I right-click the Help tile")]
        public void WhenIRightClickTheHelpTile()
        {

            //Assert.Fail("Temporary failure screenshot hook.");

            using var automation = new UIA3Automation();

            var homePage = new SimulationHomePage();
            homePage.BringToFront(automation);

            var logCollectionPage = new Test2LogCollectionPage();


            var helpButton = Retry.WhileNull(
              () => logCollectionPage.FindHelpButton(automation),
              timeout: TimeSpan.FromSeconds(30)
            ).Result;

            Assert.That(
                helpButton, Is.Not.Null,
                "The Help title was not found!");

               // Assert.Fail("Temporary failure screenshot hook.");


            Mouse.RightClick(helpButton!.BoundingRectangle.Center());
        }





        [When(@"I select ""Collect client log files""")]
        public void WhenISelectCollectClientLogFiles()
        {
            using var automation = new UIA3Automation();
            var page = new Test2LogCollectionPage();

            var menuItem = Retry.WhileNull(
               () => page.FindCollectClientLogsItem(automation),
               timeout: TimeSpan.FromSeconds(15)
            ).Result;

            Assert.That(menuItem, Is.Not.Null,
            "The Collect client log files menu item was not found.");

            Assert.That(TestEnvironment.HasAdministratorRights(), Is.True,
    "Run VS Code as administrator before starting this test.");

            _logArchive.RememberExistingArchives();
            _logCollector.RememberExistingProcesses();

            menuItem!.AsMenuItem().Invoke();

        }



        [When(@"I handle User Account Control if prompted")]
        public void WhenIHandleUserAccountControlIfPrompted()
        {
            using var collector = Retry.WhileNull(
                () => _logCollector.FindNewProcess(),
                timeout: TimeSpan.FromSeconds(30)
            ).Result;

            Assert.That(collector, Is.Not.Null,
                "Log collection did not start in this test run.");
        }



        [Then(@"the client logs should be collected successfully")]
        public void ThenTheClientLogsShouldBeCollectedSuccessfully()
        {
            var archive = Retry.WhileNull(
                () => _logArchive.FindNewArchiveWithClientLog(),
                timeout: TimeSpan.FromMinutes(5),
                interval: TimeSpan.FromSeconds(2)
            ).Result;

            Assert.That(archive, Is.Not.Null,
                $"No new or updated ZIP containing a readable client log " +
                $"was found in {_logArchive.Folder} within 5 minutes.");

            Console.WriteLine($"Verified ZIP: {archive}");
        }
    }
}