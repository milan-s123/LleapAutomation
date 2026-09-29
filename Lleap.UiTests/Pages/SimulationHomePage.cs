using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using System.Diagnostics;


namespace Lleap.UiTests.Pages
{
    public class SimulationHomePage
    {
        public AutomationElement? FindHomeTitle(UIA3Automation automation)
        {
            return automation.GetDesktop().FindFirstDescendant(
                cf => cf.ByName("Simulation Home")
                        .And(cf.ByControlType(ControlType.Text)));
        }


        public AutomationElement? FindInstructorButton(UIA3Automation automation)
{
    try
    {
        return automation.GetDesktop().FindFirstDescendant(
            cf => cf.ByName("LLEAP - Instructor Application")
                .And(cf.ByControlType(ControlType.Button)));
    }
    catch (System.Runtime.InteropServices.COMException)
    {
        return null;
    }
}


public AutomationElement? FindAddLicenseLaterButton(UIA3Automation automation)
{
    var process = Process.GetProcessesByName("InstructorApplication")
                         .FirstOrDefault();

    if (process == null)
        return null;


    var windows = automation.GetDesktop().FindAllChildren(
        cf => cf.ByProcessId(process.Id));


    foreach (var window in windows)
    {
        try
        {
            var button = window.FindFirstDescendant(
                cf => cf.ByName("Add license later")
                        .And(cf.ByControlType(ControlType.Button)));

            if (button != null)
                return button;
        }
        catch (System.Runtime.InteropServices.COMException ex)
            when (ex.HResult == unchecked((int)0x8000FFFF))
        {
            // The window is still loading; check the next window.
        }
    }

    return null;
}



public AutomationElement? FindLocalComputerTile(UIA3Automation automation)
{
    var process = Process.GetProcessesByName("InstructorApplication")
                         .FirstOrDefault();

    if (process == null)
        return null;

    var windows = automation.GetDesktop().FindAllChildren(
        cf => cf.ByProcessId(process.Id));

    foreach (var window in windows)
    {
        try
        {
            // The tile's button has no name. Its child text has the name.
            var localComputerText = window.FindFirstDescendant(
                cf => cf.ByName("Local computer")
                        .And(cf.ByControlType(ControlType.Text)));

            if (localComputerText != null)
                return localComputerText;
        }
        catch (System.Runtime.InteropServices.COMException ex)
            when (ex.HResult == unchecked((int)0x8000FFFF))
        {
            // The window is still loading; check the next window.
        }
    }

    return null;
}



public AutomationElement? FindSimMan3GPlusTile(UIA3Automation automation)
{
    var process = Process.GetProcessesByName("InstructorApplication")
                         .FirstOrDefault();

    if (process == null)
        return null;

    var windows = automation.GetDesktop().FindAllChildren(
        cf => cf.ByProcessId(process.Id));

    foreach (var window in windows)
    {
        try
        {
            // The button has no name; its child text identifies the tile.
            var tileText = window.FindFirstDescendant(
                cf => cf.ByName("SimMan 3G PLUS")
                        .And(cf.ByControlType(ControlType.Text)));

            if (tileText != null)
                return tileText;
        }
        catch (System.Runtime.InteropServices.COMException ex)
            when (ex.HResult == unchecked((int)0x8000FFFF))
        {
            // The window is still loading; check the next window.
        }
    }

    return null;
}


public AutomationElement? FindManualModeButton(UIA3Automation automation)
{
    var process = Process.GetProcessesByName("InstructorApplication")
                         .FirstOrDefault();

    if (process == null)
        return null;

    var windows = automation.GetDesktop().FindAllChildren(
        cf => cf.ByProcessId(process.Id));

    foreach (var window in windows)
    {
        try
        {
            var button = window.FindFirstDescendant(
                cf => cf.ByName("Manual Mode")
                        .And(cf.ByControlType(ControlType.Button)));

            if (button != null)
                return button;
        }
        catch (System.Runtime.InteropServices.COMException ex)
            when (ex.HResult == unchecked((int)0x8000FFFF))
        {
            // This window is still loading; try the other windows.
        }
    }

    return null;
}



public TreeItem? FindThemesItem(UIA3Automation automation)
{
    var selectThemeWindow = automation.GetDesktop().FindFirstChild(
        cf => cf.ByControlType(ControlType.Window).And(cf.ByName("Select theme")));

    var tree = selectThemeWindow?.FindFirstDescendant(
        cf => cf.ByAutomationId("theTreeView"));

    return tree?.FindFirstDescendant(
        cf => cf.ByControlType(ControlType.TreeItem).And(cf.ByName("Themes")))
        ?.AsTreeItem();
}



public TreeItem? FindHealthyPatientItem(UIA3Automation automation)
{
    var themesItem = FindThemesItem(automation);

    return themesItem?.FindFirstDescendant(
        cf => cf.ByControlType(ControlType.TreeItem)
            .And(cf.ByName("Healthy patient")))
        ?.AsTreeItem();
}




public AutomationElement? FindThemeOkButton(UIA3Automation automation)
{
    var selectThemeWindow = automation.GetDesktop().FindFirstChild(
        cf => cf.ByControlType(ControlType.Window)
            .And(cf.ByName("Select theme")));

    return selectThemeWindow?.FindFirstDescendant(
        cf => cf.ByControlType(ControlType.Button)
            .And(cf.ByAutomationId("OKButton")));
}





}
}