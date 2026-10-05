using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;

namespace Lleap.UiTests.Pages
{
    public class Test2LogCollectionPage
    {
        public AutomationElement? FindHelpButton(UIA3Automation automation)
        {
            return automation.GetDesktop().FindFirstDescendant(
                cf => cf.ByName("Help")
                        .And(cf.ByControlType(ControlType.Button)));
        }

        public AutomationElement? FindCollectClientLogsItem(
            UIA3Automation automation)
        {
            return automation.GetDesktop().FindFirstDescendant(
                cf => cf.ByName("Collect client log files")
                        .And(cf.ByControlType(ControlType.MenuItem)));
        }
    }
}