# NorthWindAPI.NET

A REST API backend for NorthWindWeb utilizing a sqlite conversion of Microsoft's North Wind Traders sample database

# Tech 

Microsoft:
+ NET Core Framework (8.0)
+ ASPNET Core Framework (8.0)
+ Entity Framework Core for SQLite
+ JWT Token Authentication
    
3rd Party:
+ Swagger via Swashbuckle
+ Automapper
+ Automapper Collections
+ NUnit (Unit Testing)
            
# Data Source

# Credits

## Database

Sourced from [GitHub Northwind SQLite3](https://github.com/jpwhite3/northwind-SQLite3)
Author: jpwhite3 

Modifications Made:
+ Additional Tables for employee logins/passwords
  - Auth(Id, RoleId, EmployeeId, Username, Hash)
  - Role(Id, RoleName)

Minor fix: 
+ CustomerId's not matching Order table
