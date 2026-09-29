using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using FlaUI.Core.Tools;

namespace Lleap.UiTests.Pages
{
    public class InstructorSessionPage
    {


        public AutomationElement? FindStartSessionButton(UIA3Automation automation)
        {
            var sessionWindow = automation.GetDesktop().FindFirstDescendant(
                cf => cf.ByControlType(ControlType.Window)
                    .And(cf.ByAutomationId("_this")));

            return sessionWindow?.FindFirstDescendant(
                cf => cf.ByControlType(ControlType.Button)
                    .And(cf.ByName("Start session")));
        }


        public AutomationElement? FindStartedSessionWindow(UIA3Automation automation)
        {
            return automation.GetDesktop().FindFirstChild(
                cf => cf.ByControlType(ControlType.Window)
                    .And(cf.ByName(
                        "Healthy patient - Virtual SimMan 3G - Manual Mode - LLEAP")));
        }


        public AutomationElement? FindEyesComboBox(UIA3Automation automation)
        {
            var sessionWindow = FindStartedSessionWindow(automation);

            return sessionWindow?.FindFirstDescendant(
                cf => cf.ByControlType(ControlType.ComboBox)
                    .And(cf.ByAutomationId("EyesComboBox")));
        }

        public AutomationElement? FindClosedEyesOption(UIA3Automation automation)
        {
            var eyesComboBox = FindEyesComboBox(automation);

            return eyesComboBox?.FindFirstDescendant(
                cf => cf.ByControlType(ControlType.ListItem)
                    .And(cf.ByName("Closed")));
        }



        public AutomationElement? FindComplianceSlider(UIA3Automation automation)
        {
            return FindStartedSessionWindow(automation)?.FindFirstDescendant(
                cf => cf.ByControlType(ControlType.Custom)
                        .And(cf.ByAutomationId("compliance")));
        }

        public AutomationElement? FindCompliance67Value(UIA3Automation automation)
        {
            return FindComplianceSlider(automation)?.FindFirstDescendant(
                cf => cf.ByControlType(ControlType.Text)
                        .And(cf.ByAutomationId("range1"))
                        .And(cf.ByName("67")));
        }



        public void ShowDetailedSessionLog(UIA3Automation automation)
        {
            var logView = FindSessionLogViewComboBox(automation)
                ?? throw new InvalidOperationException(
                    "Session log view dropdown was not found.");

            var comboBox = logView.AsComboBox();
            var selected = comboBox.Select("Detailed View");

            if (selected == null)
            {
                throw new InvalidOperationException(
                    "Detailed View option was not found.");
            }

            comboBox.Collapse();
        }

        public void SetLungComplianceTo67(UIA3Automation automation)
        {
            var slider = Retry.WhileNull(
                () => FindComplianceSlider(automation),
                timeout: TimeSpan.FromSeconds(30)
            ).Result ?? throw new InvalidOperationException(
                "Compliance slider was not found.");

            var mark = FindCompliance67Value(automation)
                ?? throw new InvalidOperationException(
                    "The 67 mark was not found.");

            var sliderBox = slider.BoundingRectangle;
            var markBox = mark.BoundingRectangle;

            // Click the track at the height of the 67 mark.
            int x = sliderBox.Left + (markBox.Left - sliderBox.Left) / 3;
            int y = markBox.Top + markBox.Height / 2;

            FlaUI.Core.Input.Mouse.Click(new System.Drawing.Point(x, y));
        }


        public int CountCompliance67LogEntries(UIA3Automation automation)
        {
            var window = FindStartedSessionWindow(automation);

            return window?.FindAllDescendants(
                cf => cf.ByControlType(ControlType.Text)
                        .And(cf.ByName("Airway compliance = 67 %"))
            ).Length ?? 0;
        }


        public AutomationElement? FindSessionLogViewComboBox(UIA3Automation automation)
        {
            return FindStartedSessionWindow(automation)?.FindFirstDescendant(
                cf => cf.ByControlType(ControlType.ComboBox)
                        .And(cf.ByAutomationId("FilterSelector")));
        }



        //And I set Patient Monitor HR to 100
        private AutomationElement OpenHeartRateDialog(UIA3Automation automation)
        {
            var window = FindStartedSessionWindow(automation)
                ?? throw new InvalidOperationException("Session window was not found.");

            var monitor = window.FindFirstDescendant(
                cf => cf.ByAutomationId("PMControl"))
                ?? throw new InvalidOperationException("Patient Monitor was not found.");

            var hr = monitor.FindFirstDescendant(
                cf => cf.ByControlType(ControlType.Pane).And(cf.ByAutomationId("11")))
                ?? throw new InvalidOperationException("HR control was not found.");

            hr.Click();

            return Retry.WhileNull(
                () => automation.GetDesktop().FindFirstDescendant(
                    cf => cf.ByName("Set Heart Rate")
                        .And(cf.ByProcessId(window.Properties.ProcessId.Value))),
                timeout: TimeSpan.FromSeconds(5)
            ).Result ?? throw new InvalidOperationException("Set Heart Rate did not open.");
        }

        public string SetPatientMonitorHrTo100(UIA3Automation automation)
        {
            var dialog = OpenHeartRateDialog(automation);

            var input = dialog.FindFirstDescendant(
                cf => cf.ByControlType(ControlType.Edit).And(cf.ByAutomationId("2093")))
                ?? throw new InvalidOperationException("HR input was not found.");

            input.AsTextBox().Text = "100";

            var ok = dialog.FindFirstDescendant(
                cf => cf.ByControlType(ControlType.Button).And(cf.ByAutomationId("1")))
                ?? throw new InvalidOperationException("OK button was not found.");

            ok.Click();

            // Reopen the dialog to read the applied current value.
            dialog = OpenHeartRateDialog(automation);

            var currentValue = dialog.FindFirstDescendant(
                cf => cf.ByControlType(ControlType.Text).And(cf.ByAutomationId("2103")))
                ?? throw new InvalidOperationException("Current HR value was not found.");

            string actualHr = currentValue.Name;

            dialog.FindFirstDescendant(
                cf => cf.ByControlType(ControlType.Button).And(cf.ByAutomationId("2")))
                ?.Click();

            return actualHr;
        }



        public bool PlayCoughingOnce(UIA3Automation automation)
{
    ShowDetailedSessionLog(automation);

    var window = FindStartedSessionWindow(automation)
        ?? throw new InvalidOperationException("Session window was not found.");

    var soundPanel = window.FindFirstDescendant(
        cf => cf.ByAutomationId("VocalSoundPanel"))
        ?? throw new InvalidOperationException("Vocal sound panel was not found.");

    var soundTree = soundPanel.FindFirstDescendant(
        cf => cf.ByAutomationId("VocalSoundTreeView"))
        ?? throw new InvalidOperationException("Vocal sound list was not found.");

    var coughing = soundTree.FindFirstDescendant(
        cf => cf.ByControlType(ControlType.TreeItem).And(cf.ByName("Coughing")))
        ?? throw new InvalidOperationException("Coughing was not found.");

    var play = soundPanel.FindFirstDescendant(
        cf => cf.ByAutomationId("PlayButton"))
        ?? throw new InvalidOperationException("Play button was not found.");

    int CountPlays() => window.FindAllDescendants(
        cf => cf.ByControlType(ControlType.Text)
            .And(cf.ByName("Vocal sound played: Coughing"))).Length;

    int entriesBefore = CountPlays();

    coughing.Click();
    play.Click();

    return Retry.WhileNull(
        () => CountPlays() > entriesBefore ? "Confirmed" : null,
        timeout: TimeSpan.FromSeconds(5)
    ).Result != null;
}




public void CloseInstructorApplication(UIA3Automation automation)
{
    var window = FindStartedSessionWindow(automation)
        ?? throw new InvalidOperationException("Session window was not found.");
    window.AsWindow().Close();
}

public bool WaitUntilInstructorApplicationIsClosed(UIA3Automation automation)
{
    return Retry.WhileNull(
        () => FindStartedSessionWindow(automation) == null
            ? "Closed"
            : null,
        timeout: TimeSpan.FromSeconds(10),
        ignoreException: true
    ).Result != null;
}


    }
}