# LLEAP UI Test Automation

This project contains two automated UI tests for LLEAP.

## Technologies

- C#
- .NET 10
- Reqnroll 3.3.4
- NUnit 4.3.2
- FlaUI 5.0.0 with UIA3


## Setup

- Install LLEAP and follow the hardware and operating system requirements
  listed on the Laerdal website.
- Install the .NET 10 SDK.
- Install Visual Studio Code with the C# Dev Kit extension.

The project was developed on Windows 11 ARM using Parallels Desktop on a Mac.

The application path used in this project is:

```text
C:\Program Files (x86)\Laerdal Medical\Laerdal Simulation Home\LaunchPortal.exe
```

If the installation path is different, update `FileName` and
`WorkingDirectory` in `Utilities/TestEnvironment.cs`.

`FileName` is the full path to `LaunchPortal.exe`.
`WorkingDirectory` is the folder containing that file.


## Initial application setup

The tests use the LLEAP application in English.

Follow these preparation steps from the assignment manually once after
installation, before running the automated tests:

1. Start Laerdal Simulation Home.
2. Open the Instructor Application.
3. Select "Add license later" when prompted.
4. Select Local Computer under Virtual Simulator.
5. Select SimMan3G Plus.
6. Select "or continue without a debriefing system."
7. Select International Preferences.
8. Select Manual Mode.
9. Expand the Themes list.
10. Select Healthy Patient.
11. Click OK in the bottom right corner.
12. Click Start Session.
13. Maximize the window.
14. Close the Instructor Application using the X button.

This setup helps prevent extra setup windows from interrupting the tests.


## Administrator rights

1. Open Visual Studio Code using "Run as administrator".
2. Accept the Windows User Account Control prompt.
3. Open the project folder.

Both tests can be run from this administrator session.

Test 2 needs administrator rights to collect logs.
Running Visual Studio Code as administrator allows log collection
to start without another User Account Control prompt.

The test checks that the log collector starts.
It does not click the User Account Control prompt.


## Test 1: Virtual SimMan3G Plus session

The test opens the Instructor Application without a license.

It selects:

- Local Computer under Virtual Simulator
- SimMan3G Plus
- Manual Mode
- Healthy Patient

After starting the session, it:

- Maximizes the Instructor Application.
- Sets Eyes to Closed.
- Sets Lung compliance to 67 percent.
- Sets Patient Monitor HR to 100.
- Plays Coughing once.
- Closes the Instructor Application and checks that it is closed.


## Test 2: Client log collection

The test right-clicks the Help tile and selects "Collect client log files".

It checks that:

- A new log collector process starts.
- A new or updated ZIP archive is available.
- The ZIP contains a readable, non-empty client log.

The ZIP location is:

```text
C:\Users\Public\Documents\Laerdal Report Zipped
```

The ZIP archive created by Test 2 is kept after the test.


## Running the tests in Test Explorer

1. Open Visual Studio Code as administrator.
2. Open the project folder.
3. Save any changes before running a test.
4. Open the Testing panel on the left.
5. Expand the test list.
6. Click Run Test next to the scenario you want to run.
7. Wait for it to finish before starting the other test.

Run one test at a time.
Do not lock Windows or let it go to sleep while a test is running.
Avoid using the mouse or keyboard during the test.

Before running Tests, close any remaining LLEAP, Simulation Engine and Voice Conference windows from the previous session.


## Test results and hooks

Results are saved in:

```text
Lleap.UiTests/bin/Debug/net10.0-windows/TestResults/
```

Each scenario has its own folder named with the date and time.

- `BeforeScenario` creates the scenario folder inside `TestResults`.
- `AfterStep` writes the status and text of each executed step to `steps.txt`.
  If a step fails, it also saves the error message and captures `failure.png`.
- `AfterScenario` closes the Simulation Home process started by the test
  and releases the saved Process object.


## Project structure

- `Resources/Features`: Gherkin scenarios.
- `StepDefinitions`: code for the scenario steps.
- `Pages`: methods for finding and using UI elements.
- `Utilities`: application startup, administrator rights checks,
  log collector monitoring and ZIP checks.
- `Hooks`: BeforeScenario, AfterStep and AfterScenario methods.