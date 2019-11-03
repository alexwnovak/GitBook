Feature: Editing commits
   As a programmer
   I want to commit my changes
   So they can be incorporated into the application's source code

Scenario: Creating a new commit
   Given I am editing a new commit
   And I have entered Fixing bugs into the subject field
   And I have entered the following lines into the body field:
   | Line              |
   | Fixing issue #001 |
   | Fixing issue #002 |
   | Fixing issue #002 |
   When I save the commit
   Then the commit data is written to the commit file
   And the window is closed

Scenario: Saving the commit when prompted after discarding with changes
   Given I am editing a new commit
   And I have entered Fixing bugs into the subject field
   And I have entered the following lines into the body field:
   | Line              |
   | Fixing issue #001 |
   | Fixing issue #002 |
   | Fixing issue #002 |
   When I discard the commit and press Yes
   Then the commit data is written to the commit file
   And the window is closed

Scenario: Amending a commit
   Given I am amending an existing commit
   | Field   | Value            |
   | Subject | Fixng bugs       |
   | Body    | Fixng issue #001 |
   And I change the subject to Fixing
   And I change the body to:
   | Line              |
   | Fixing issue #001 |
   | Fixing issue #002 |
   When I save the commit
   Then the commit data is written to the commit file
   And the window is closed

Scenario: Discarding a new commit
   Given I am editing a new commit
   And I have entered Fixing a bug into the subject field
   And I have entered the following lines into the body field:
   | Line              |
   | Fixing issue #001 |
   | Fixing issue #002 |
   | Fixing issue #002 |
   When I discard the commit
   Then blank commit data is written to the commit file
   And the window is closed
