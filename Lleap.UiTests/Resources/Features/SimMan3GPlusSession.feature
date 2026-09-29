Feature: Run a virtual SimMan3G Plus session without a license

  As a user of LLEAP
  I want to start a virtual SimMan3G Plus session without a license
  So that I can use the simulator controls

Scenario: Start a session and configure the simulator
	Given I start Laerdal Simulation Home
	When I open the Instructor Application
	And I choose "Add license later" if prompted
	And I select Local Computer under Virtual Simulator
	And I select SimMan3G Plus
	And I select Manual Mode
	And I expand the Themes list
	And I select Healthy Patient
	And I confirm the theme with OK
	And I start the session
	And I maximize the Instructor Application window
	And I set Eyes to Closed
	And I set Lung compliance to 67 percent
	And I set Patient Monitor HR to 100
	And I select Coughing under Voices and play it once
	And I close the Instructor Application
	Then the Instructor Application is closed






