# HRAS_Codebase
NOTE: all names/passwords in the SQL and C# code can be changed but they must match through out the databaseSqlCode.sql.text and C# Middlewere properties settings1.settings

First time Startup procedure:
1. Go into Database folder and Click on the databaseSqlCode.sql.text file
2. Open up Microsodft SQL Server Managment
3. Right click on the sever and open a New Query
4. Paste the code you got from databaseSqlCode.sql.text if you wish to make changes now is the time to do so
5. Make sure that the C# Middlewere/properties/settings1.settings has the same data in the script
6. Execute the code
7. Open up the Security at the sever leven and find the user HRAS_MW_iTas and right click and select the properties and change the defalt database to HRAS_iTas
8. At the database level open security and find the user you create and alow to write from database
9. Run the C# program
10. login to Username = admin and Password = password      p.s. this is just a temp user you should delete this user
11. Click on "Import Files" button
12. Click on "Browse For File" and find a file you wish to import and select what kind of file it is and hit "Begin Import" but always import the room file before others
13. Repeat 11 until all file have been imported
14. Now the system is ready for use
