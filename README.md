# FXV management system

## [Live System (Beta)](https://www.fxv.co.nz/).

NOTE:the LINK MAY TAKE A WHILE TO LOAD - you may have to be a little patient, but it will get there eventually!
Due to the system hosted on a testing server, not ready for commercial as currently the system is still under the developing stage.

## Features:
1. Account management system including login, registration, password reset.
2. Athlete data management for organization and team level.
3. Data visualization
4. Leaderboard makes athlete feeling competible with each other.
5. Running live event with combined tests for athlete and collecting data for performance analysis.
6. Search function applied in most of page increased usability of system.
7. Security solution keeps all athlete data secured.
8. Pre-defined system permission for different users for different amount of information.

## Upcoming Features:
1. New permission strategy that allows admin to define system permission based on demanding.
2. Public leaderboard on the event
3. Hardware integration for easy data collection
4. Content management system to manage all essential file for subscription users, like training videos and professional statistics files.
5. Bulk data importing

## Running Environment

## Requirements:

###### Runtime requirements: APS.Net Core 2.2
###### OS: Windows Server or Linux based OS

## Step 1:

###### Download project via gitbub

## Step 2:

###### On server:
###### Complile/Publish entire system, and upload to server.
###### Local server:
###### Open the project on Visual studio, or other compatible IDE

## Step 3:

###### Database update
###### Hint: project contains all migration files, only need to update database.
###### CLI: dotnet ef database update
###### Visual studio: Update-Database

## Step 4:
###### Runs the application in development mode.
###### Browse https://localhost:5001 to view the system.


