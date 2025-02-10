# NorthWindAPI.NET

A REST API backend for [NorthWindWeb](https://github.com/SuiteBee/NorthWindWeb) utilizing a sqlite conversion of Microsoft's North Wind Traders sample database.

The primary goal of this project was to learn React. As such, this portion acted as more of an additional technical challenge to overcome communicating with an external API.

All endpoints viewable [here](https://suitebee.github.io/NorthWindAPI.NET/#/)

# Features

### A short summary of the operations performable on the data source

+ Customers
    - Return all + Find by ID + Regions + Create new + Update existing

+ Dashboard
    - Return chart data
  
+ Order
    - Return all + Find by ID + Create new + Ship single + Ship many + Return carriers + Delete by ID

+ Product
    - Return all + Find by ID + Update price + Update stock + Update record

+ User
    - Authenticate + Update credentials
  
### Security

+ Default password for all users is **Northwind1**
+ Includes password hashing for updating and storing user credentials
+ Stores password as unique hash in the database for each user despite passwords being identical
+ At login, the API will genearte and return a JWT token to authorize future requests
+ This token is valid for 30 minutes

![auth_table](https://github.com/user-attachments/assets/4bf3dbf2-5fc7-4ab6-b619-d4acc400b447)

# Tech 

+ Microsoft
    - NET Core Framework (8.0)
    - ASPNET Core Framework (8.0)
    - Entity Framework Core for SQLite
    - JWT Token Authentication
    
+ 3rd Party
    - Swagger via Swashbuckle
    - Automapper
    - Automapper Collections
    - NUnit (Unit Testing)
            
# Data Source

> [!NOTE]
> Sqlite was chosen due to its light weight and simplicity which proved good enough for a hobby project
> 
> Would not reccomend this database to be used in a real world scenario for an API receiving many requests from multiple sources

![northwind_erd](https://github.com/user-attachments/assets/bfa7c4ad-c730-4008-a596-81706343fb77)

Sourced from [GitHub Northwind SQLite3](https://github.com/jpwhite3/northwind-SQLite3)

Modifications Made:
+ Additional Tables for employee logins/passwords
  - Auth(Id, RoleId, EmployeeId, Username, Hash)
  - Role(Id, RoleName)

Minor fix: 
+ CustomerId's not matching Order table
