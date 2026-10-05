Feature: Test 2 - Collect client logs

  As a user of LLEAP
  I want to collect client log files
  So that I can help identify problems

Scenario: Collect client log files successfully
	Given I start Laerdal Simulation Home
	When I right-click the Help tile
	And I select "Collect client log files"
	And I handle User Account Control if prompted
	Then the client logs should be collected successfully