# Practical 22 - Employee API

This repository contains a simple ASP.NET Core Web API backed by a class library (DAL) that performs SQL operations for employee records. The solution implements a singleton database service, a deferred-loading repository, and a file-based logger (eagerly loaded) to meet the practical requirements.

## Solution Structure

- **EmployeeAPI**: ASP.NET Core Web API project with the `EmployeeController` exposing CRUD-style endpoints.
- **EmployeeDAL**: Class library containing data access classes (`DbService`, `EmployeeRepository`) and models/DTOs.
- **Practical-22.sln**: Solution file.

## Key Implementation Details

- **Singleton**: `DbService` exposes a lazily initialized singleton for SQL connection creation.
- **Deferred Loading**: `EmployeeRepository` fetches a new connection per operation via `DbService.Instance.GetConnection()`.
- **Eager Loading Logger**: `FileLoggerService` is registered as a singleton and writes logs to `log.txt`.

## Database Schema

The API expects an `Employee` table similar to the following (identity primary key, with a status column for soft delete):

```sql
CREATE TABLE Employee (
    EmployeeId INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeName NVARCHAR(200) NOT NULL,
    EmployeeSalary DECIMAL(18,2) NOT NULL,
    DepartmentId INT NOT NULL,
    EmployeeEmail NVARCHAR(200) NOT NULL,
    EmployeeJoiningDate DATETIME NOT NULL,
    EmployeeStatus NVARCHAR(50) NOT NULL
);
```

> The default connection string is configured in `EmployeeDAL/Data/DbService.cs` as:
> `Server=(LocalDb)\MSSQLLocalDb;Database=Practical22;Trusted_Connection=True;`

## API Endpoints

Base route: `api/Employee`

### 1) Create Employee
- **POST** `/api/Employee`
- **Body**:
  ```json
  {
    "employeeName": "Jane Doe",
    "employeeSalary": 75000,
    "departmentId": 1,
    "employeeEmail": "jane@example.com",
    "employeeJoiningDate": "2024-02-01T00:00:00"
  }
  ```

### 2) Update Employee
- **PUT** `/api/Employee`
- **Body**:
  ```json
  {
    "employeeId": 1,
    "employeeName": "Jane Doe",
    "employeeSalary": 80000,
    "departmentId": 2,
    "employeeEmail": "jane@example.com",
    "employeeJoiningDate": "2024-02-01T00:00:00"
  }
  ```

### 3) Soft Delete (Deactivate)
- **DELETE** `/api/Employee/{id}`
- **Effect**: Sets `EmployeeStatus` to `InActive`.

### 4) Get Employees
- **GET** `/api/Employee` → returns all employees.
- **GET** `/api/Employee/{id}` → returns a single employee by ID.

## Running the API

1. Ensure SQL Server LocalDB is available and the `Practical22` database with the `Employee` table exists.
2. From the repository root:
   ```bash
   dotnet restore
   dotnet run --project EmployeeAPI
   ```
3. Navigate to the Swagger UI (in Development) at `https://localhost:<port>/swagger`.

## Logging

All controller actions write to `log.txt` in the application root using the `FileLoggerService` singleton.

## Notes

- Department values are expected to match your own mapping (e.g., IT, Admin, HR, Sales, On-site) via the integer `DepartmentId` column.
- The `EmployeeStatus` field is used to represent active/inactive employees for soft delete.
