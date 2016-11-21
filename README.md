## Example Project Management Web Application ##

Created for the BCIT COMP 3900 Projects Course.

This application allows users within an organization log their weekly work contributions according to a 40 hour work week.

Administrators are able to add new projects and add customized work categories for each project.

Users of the tool will be able to log what contributions were made during the week under the respective project and work categories.

Administrators will be able to see what the weekly, monthly, or total contributions were made by each employee. In addtion, they will be able
to identify which users have not logged enough hours for the 40 hour work week.

Setting Up Your Server and Database for Migrations
--------------------------------------------------
The project's Web.Config contains the connection string for the database you will point towards.

Below is an example connection string to a SQL Server instance:

\<add name="SREDContext" connectionString="Server=SERVER_NAME; Database=TARGET_DATABASE_NAME; User ID=USERNAME; Password=PASSWORD; Trusted_Connection=False; Encrypt=True" providerName="System.Data.SqlClient"/\>"

Once your connection string is set, open the "Package Manager Console" in Visual Studio and input the following command to migrate to the latest version of the database:
update-database (you may add the -Verbose flag to see what SQL queries the Entity framework is sending to the server.

The database will be seeded with only the most essential data.

Switching between Migrations and LocalDB
----------------------------------------
When you are using the application in production, you must make sure to disable the LocalDB initializer.<br/>
To do so, go to the Web.Config file and comment out the \<contexts\>\</contexts\> section and comment out the connection string that points to LocalDb<br/>
<br/>
To re-enable the LocalDB initializer for debugging and development, simply uncomment the two sections and comment out the connection string pointing to your production database.<br/>

Manually Adding Employee Positions
----------------------------------
Currently, there is no method of creating a new Employee Position inside the SR&ED Tool.

The table schema for an employee Position is simply:

PositionID, Guid<br />
PositionName, varchar<br />

By accessing your SQL Server instance, you may insert any new positions as necessary using:

INSERT INTO Position(PositionID, PositionName)<br />
VALUES (NEWID(), 'Insert Position Name Here')
