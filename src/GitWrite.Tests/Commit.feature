Feature: Editing commits
   As a programmer
   I want to commit my changes
   So they can be incorporated into the application's source code

Scenario: Creating a new commit
   Given I am editing a new commit
   And I have entered Fixing a bug into the subject field
   When I save the commit
   Then the commit data is written to the commit file
   And the window is closed
