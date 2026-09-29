using System;
using Reqnroll;
using FlaUI.Core.Definitions;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using NUnit.Framework;
using Lleap.UiTests.Pages;
using FlaUI.Core.AutomationElements;


namespace Lleap.UiTests.StepDefinitions
{
		[Binding]
		public class SimMan3GPlusSessionStepDefinitions
		{
			private readonly ScenarioContext _scenarioContext;
	
			public SimMan3GPlusSessionStepDefinitions(ScenarioContext scenarioContext)
			{
				_scenarioContext = scenarioContext;
			}


			

[Given(@"I start Laerdal Simulation Home")]
public void GivenIstartLaerdalSimulationHome()
{
	System.Diagnostics.Process.Start(
    @"C:\Program Files (x86)\Laerdal Medical\Laerdal Simulation Home\LaunchPortal.exe");

	using var automation = new UIA3Automation();
    var homePage = new SimulationHomePage();

    var homeTitle = Retry.WhileNull(
        () => homePage.FindHomeTitle(automation),
        timeout: TimeSpan.FromSeconds(60)
    ).Result;

Assert.That(homeTitle, Is.Not.Null, "Laerdal Simulation Home did not open.");
}



[When(@"I open the Instructor Application")]
public void WhenIopentheInstructorApplication()
{
    using var automation = new UIA3Automation();
    var homePage = new SimulationHomePage();

    var instructorButton = Retry.WhileNull(
        () => homePage.FindInstructorButton(automation),
        timeout: TimeSpan.FromSeconds(15)
    ).Result;

    Assert.That(instructorButton, Is.Not.Null,
        "Instructor Application button was not found.");

    instructorButton!.AsButton().Invoke();

    var licenseButton = Retry.WhileNull(
        () => homePage.FindAddLicenseLaterButton(automation),
        timeout: TimeSpan.FromSeconds(30)
    ).Result;

    Assert.That(licenseButton, Is.Not.Null,
        "Instructor did not reach the license screen after Invoke.");
}




[When(@"I choose ""Add license later"" if prompted")]
public void WhenIChooseAddLicenseLaterIfPrompted()
{
    using var automation = new UIA3Automation();
    var homePage = new SimulationHomePage();

    var addLicenseLaterButton = Retry.WhileNull(
        () => homePage.FindAddLicenseLaterButton(automation),
        timeout: TimeSpan.FromSeconds(30)
    ).Result;
	
Assert.That(addLicenseLaterButton, Is.Not.Null,
    "Add license later button was not found.");
addLicenseLaterButton!.AsButton().Invoke();
}




[When(@"I select Local Computer under Virtual Simulator")]
public void WhenISelectLocalComputerUnderVirtualSimulator()
{
    using var automation = new UIA3Automation();
    var homePage = new SimulationHomePage();

    var localComputerText = Retry.WhileNull(
        () => homePage.FindLocalComputerTile(automation),
        timeout: TimeSpan.FromSeconds(30)
    ).Result;

    Assert.That(localComputerText, Is.Not.Null,
        "Local computer tile was not found.");

    localComputerText!.Click();
}




[When(@"I select SimMan3G Plus")]
public void WhenISelectSimMan3GPlus()
{
    using var automation = new UIA3Automation();
    var homePage = new SimulationHomePage();

    var simManTileText = Retry.WhileNull(
        () => homePage.FindSimMan3GPlusTile(automation),
        timeout: TimeSpan.FromSeconds(30)
    ).Result;

    Assert.That(simManTileText, Is.Not.Null,
        "SimMan 3G PLUS tile was not found.");

    simManTileText!.Click();
}




[When(@"I select Manual Mode")]
public void WhenISelectManualMode()
{
    using var automation = new UIA3Automation();
    var homePage = new SimulationHomePage();

    var manualModeButton = Retry.WhileNull(
        () => homePage.FindManualModeButton(automation),
        timeout: TimeSpan.FromSeconds(30)
    ).Result;

    Assert.That(manualModeButton, Is.Not.Null,
        "Manual Mode button was not found.");

    manualModeButton!.Click();

}



[When(@"I expand the Themes list")]
public void WhenIExpandTheThemesList()
{
    using var automation = new UIA3Automation();
    var homePage = new SimulationHomePage();

    var themesItem = Retry.WhileNull(
        () => homePage.FindThemesItem(automation),
        timeout: TimeSpan.FromSeconds(30)
    ).Result;
    Assert.That(themesItem, Is.Not.Null, "Themes list was not found.");


    if (themesItem!.ExpandCollapseState == ExpandCollapseState.Collapsed)
    {
        themesItem.Expand();
    }
    Assert.That(
        themesItem.ExpandCollapseState,
        Is.EqualTo(ExpandCollapseState.Expanded),
        "Themes list did not expand.");
}



		

[When(@"I select Healthy Patient")]
public void GivenISelectHealthyPatient()
{
    using var automation = new UIA3Automation();
    var homePage = new SimulationHomePage();

    var healthyPatient = Retry.WhileNull(
        () => homePage.FindHealthyPatientItem(automation),
        timeout: TimeSpan.FromSeconds(30)
    ).Result;
    Assert.That(healthyPatient, Is.Not.Null, "Healthy patient theme was not found.");


    healthyPatient!.Select();

    Assert.That(healthyPatient.IsSelected, Is.True,
        "Healthy patient theme was not selected.");
}





[When(@"I confirm the theme with OK")]
public void WhenIConfirmTheThemeWithOk()
{
    using var automation = new UIA3Automation();
    var homePage = new SimulationHomePage();

    var okButton = Retry.WhileNull(
        () => homePage.FindThemeOkButton(automation),
        timeout: TimeSpan.FromSeconds(30)
    ).Result;

    Assert.That(okButton, Is.Not.Null,
        "OK button was not found in Select theme.");

    okButton!.Click();
}




		}
	}