using System;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using NUnit.Framework;
using Reqnroll;
using Lleap.UiTests.Pages;
using FlaUI.Core.Definitions;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;

namespace Lleap.UiTests.StepDefinitions
{
    [Binding]
    public class InstructorSessionStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public InstructorSessionStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }



        [When(@"I start the session")]
        public void WhenIStartTheSession()
        {
            using var automation = new UIA3Automation();
            var sessionPage = new InstructorSessionPage();

            var startSessionButton = Retry.WhileNull(
     () => sessionPage.FindStartSessionButton(automation),
     timeout: TimeSpan.FromSeconds(30),
     interval: TimeSpan.FromSeconds(1),
     ignoreException: true
             ).Result;
            Assert.That(startSessionButton, Is.Not.Null,
                "Start session button was not found.");

            startSessionButton!.AsButton().Invoke();

            var startedSessionWindow = Retry.WhileNull(
                () => sessionPage.FindStartedSessionWindow(automation),
                timeout: TimeSpan.FromSeconds(30)
            ).Result;
            Assert.That(startedSessionWindow, Is.Not.Null,
                "The session did not start: the active Instructor window was not found.");
        }




        [When(@"I maximize the Instructor Application window")]
        public void WhenIMaximizeTheInstructorApplicationWindow()
        {
            using var automation = new UIA3Automation();
            var sessionPage = new InstructorSessionPage();

            var sessionWindow = Retry.WhileNull(
                () => sessionPage.FindStartedSessionWindow(automation),
                timeout: TimeSpan.FromSeconds(30)
            ).Result;
            Assert.That(sessionWindow, Is.Not.Null,
                "Instructor Application window was not found.");

            sessionWindow!.Patterns.Window.Pattern.SetWindowVisualState(
                WindowVisualState.Maximized);

            Assert.That(
                sessionWindow.Patterns.Window.Pattern.WindowVisualState.Value,
                Is.EqualTo(WindowVisualState.Maximized),
                "Instructor Application window was not maximized.");
        }




        [When(@"I set Eyes to Closed")]
        public void WhenISetEyesToClosed()
        {
            using var automation = new UIA3Automation();
            var sessionPage = new InstructorSessionPage();

            var eyesElement = Retry.WhileNull(
                () => sessionPage.FindEyesComboBox(automation),
                timeout: TimeSpan.FromSeconds(30)
            ).Result;
            Assert.That(eyesElement, Is.Not.Null, "Eyes dropdown was not found.");

            eyesElement!.Click();

            var closedOption = Retry.WhileNull(
                () => sessionPage.FindClosedEyesOption(automation),
                timeout: TimeSpan.FromSeconds(5)
            ).Result;
            Assert.That(closedOption, Is.Not.Null, "Closed option was not found.");

            closedOption!.Click();

            var selectedEyes = sessionPage.FindEyesComboBox(automation);
            Assert.That(selectedEyes?.Name, Is.EqualTo("Closed"),
                "Eyes were not set to Closed.");

        }




        [When(@"I set Lung compliance to 67 percent")]
        public void WhenISetLungComplianceTo67Percent()
        {
            using var automation = new UIA3Automation();
            var sessionPage = new InstructorSessionPage();

            sessionPage.ShowDetailedSessionLog(automation);
            int entriesBefore = sessionPage.CountCompliance67LogEntries(automation);

            sessionPage.SetLungComplianceTo67(automation);

            var confirmation = Retry.WhileNull(
                () => sessionPage.CountCompliance67LogEntries(automation) > entriesBefore
                    ? "Confirmed"
                    : null,
                timeout: TimeSpan.FromSeconds(5)
            ).Result;

            Assert.That(confirmation, Is.Not.Null,
                "No new 'Airway compliance = 67 %' entry appeared after the click.");

        }





        [When(@"I set Patient Monitor HR to 100")]
        public void WhenISetPatientMonitorHrTo100()
        {
            using var automation = new UIA3Automation();
            var sessionPage = new InstructorSessionPage();

            string actualHr = sessionPage.SetPatientMonitorHrTo100(automation);

            Assert.That(actualHr, Is.EqualTo("100"),
                "Patient Monitor HR was not set to 100.");
        }





        [When(@"I select Coughing under Voices and play it once")]
        public void WhenISelectCoughingUnderVoicesAndPlayItOnce()
        {
            using var automation = new UIA3Automation();
            var sessionPage = new InstructorSessionPage();

            Assert.That(sessionPage.PlayCoughingOnce(automation), Is.True,
                "No new Coughing playback entry appeared in the session log.");
        }




        [When(@"I close the Instructor Application")]
        public void WhenICloseTheInstructorApplication()
        {
            using var automation = new UIA3Automation();
            var sessionPage = new InstructorSessionPage();

            sessionPage.CloseInstructorApplication(automation);
        }





        [Then(@"the Instructor Application is closed")]
        public void ThenTheInstructorApplicationIsClosed()
        {
            using var automation = new UIA3Automation();
            var sessionPage = new InstructorSessionPage();

            Assert.That(
                sessionPage.WaitUntilInstructorApplicationIsClosed(automation),
                Is.True,
                "Instructor Application window is still open.");

        }

    }
}