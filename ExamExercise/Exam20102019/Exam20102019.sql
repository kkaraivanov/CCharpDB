/* 
CREATE DATABASE Service
GO
USE Service
GO
*/

CREATE TABLE Users(
    Id INT PRIMARY KEY IDENTITY,
    Username VARCHAR(30) UNIQUE NOT NULL,
    [Password] VARCHAR(50) NOT NULL,
    [Name] VARCHAR(50),
    Birthdate DATETIME2,
    Age INT CHECK(Age BETWEEN 14 AND 110),
    Email VARCHAR(50) NOT NULL
)

CREATE TABLE Departments(
    Id INT PRIMARY KEY IDENTITY,
    [Name] VARCHAR(50) NOT NULL
)

CREATE TABLE Employees(
    Id INT PRIMARY KEY IDENTITY,
    FirstName VARCHAR(25),
    LastName VARCHAR(25),
    Birthdate DATETIME2,
    Age INT CHECK(Age BETWEEN 18 AND 110),
    DepartmentId INT FOREIGN KEY REFERENCES Departments(Id)
)

CREATE TABLE Categories(
    Id INT PRIMARY KEY IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    DepartmentId INT FOREIGN KEY REFERENCES Departments(Id) NOT NULL
)

CREATE TABLE Status(
    Id INT PRIMARY KEY IDENTITY,
    [Label] VARCHAR(30) NOT NULL,
)

CREATE TABLE Reports(
    Id INT PRIMARY KEY IDENTITY,
    CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
    StatusId INT FOREIGN KEY REFERENCES Status(Id) NOT NULL,
    OpenDate DATETIME2 NOT NULL,
    CloseDate DATETIME2,
    [Description] VARCHAR(200) NOT NULL,
    UserId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL,
    EmployeeId INT FOREIGN KEY REFERENCES Employees(Id)
)

/* 2.	Insert */
INSERT INTO Employees(FirstName, LastName, Birthdate, DepartmentId)
VALUES
('Marlo', 'O''Malley', 1958-9-21, 1),
('Niki', 'Stanaghan', 1969-11-26, 4),
('Ayrton', 'Senna',	1960-03-21, 9),
('Ronnie', 'Peterson',	1944-02-14, 9),
('Giovanna', 'Amati', 1959-07-20, 5)

INSERT INTO Reports(CategoryId, StatusId, OpenDate, CloseDate, Description, UserId, EmployeeId)
VALUES
(1, 1, 2017-04-13, NULL, 'Stuck Road on Str.133', 6, 2),
(6, 3, 2015-09-05, 2015-12-06, 'Charity trail running', 3, 5),
(14, 2, 2015-09-07, NULL, 'Falling bricks on Str.58', 5, 2),
(4, 3, 2017-07-03, 2017-07-06, 'Cut off streetlight on Str.11', 1, 1)

UPDATE Reports
SET CloseDate = GETDATE()
WHERE CloseDate IS NULL

/* 4.	Delete */
DELETE FROM Reports
where StatusId = 4

/* 5.	Unassigned Reports */
SELECT r.[Description], FORMAT(r.OpenDate, 'dd-MM-yyyy') [OpenDate] FROM Reports r
WHERE EmployeeId IS NULL
ORDER BY r.OpenDate ASC, [Description] ASC

/* 6.	Reports & Categories */
SELECT r.Description, c.Name [CategoryName] FROM Reports r
RIGHT JOIN Categories c ON r.CategoryId = c.Id
WHERE r.Id IS NOT NULL
ORDER BY Description, CategoryName

/* 7.	Most Reported Category */
SELECT TOP(5) c.Name [CategoryName], COUNT(c.Id) [ReportsNumber] FROM Categories c
LEFT JOIN Reports r ON c.Id = r.CategoryId
WHERE r.Id IS NOT NULL
GROUP BY c.Name
ORDER BY ReportsNumber DESC, CategoryName ASC

/* 8.	Birthday Report */
SELECT u.Username, c.Name [CategoryName] FROM Users u
LEFT JOIN Reports r ON u.Id = r.UserId
JOIN Categories c ON r.CategoryId = c.Id
WHERE 
	DATEPART(MONTH, u.Birthdate) = DATEPART(MONTH, r.OpenDate) AND 
	DATEPART(DAY, u.Birthdate) = DATEPART(DAY, r.OpenDate)
ORDER BY u.Username, CategoryName

/* 9.	Users per Employee  */
SELECT e.FirstName + ' ' + e.LastName [FullName], COUNT(r.UserId) [UsersCount] FROM Employees e
LEFT JOIN Reports r ON e.Id = r.EmployeeId
GROUP BY e.FirstName, e.LastName
ORDER BY UsersCount DESC, FullName ASC

/* 10.	Full Info */
SELECT 
	ISNULL(e.FirstName + ' ' + e.LastName, 'None') [Employee],
	ISNULL(d.Name, 'None') [Department],
	ISNULL(c.Name, 'None') [Category],
	r.Description [Description],
	FORMAT(r.OpenDate, 'dd.MM.yyyy') [OpenDate],
	s.Label [Status],
	u.Name [User]
FROM Reports r
LEFT JOIN Employees e ON e.Id = r.EmployeeId
LEFT JOIN Users u ON r.UserId = u.Id
LEFT JOIN Departments d ON e.DepartmentId = d.Id
LEFT JOIN Categories c ON r.CategoryId = c.Id
LEFT JOIN Status s ON r.StatusId = s.Id
ORDER BY 
	e.FirstName DESC,
	e.LastName DESC,
	Department ASC,
	Category ASC,
	Description ASC,
	r.OpenDate ASC,
	Status ASC,
	User ASC

GO
/* 11.	Hours to Complete */
CREATE OR ALTER FUNCTION dbo.udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
RETURNS VARCHAR(10) AS
BEGIN
	DECLARE @result VARCHAR(10)
	IF(@StartDate IS NULL OR @EndDate IS NULL)
		BEGIN
			SET @result = '0'
		END
	ELSE
		BEGIN
			SET @result = DATEDIFF(HOUR, @StartDate, @EndDate)
		END

	RETURN @result
END
GO
SELECT dbo.udf_HoursToComplete(OpenDate, CloseDate) AS TotalHours
   FROM Reports

GO
/* 12.	Assign Employee */
CREATE PROCEDURE usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT) AS
BEGIN
	DECLARE @employeeDepartmentId INT = (
		SELECT d.Id FROM Employees e
		JOIN Departments d ON e.DepartmentId = d.Id
		WHERE e.Id = @EmployeeId)
	DECLARE @reportDepartmentId INT = (
		SELECT d.Id FROM Reports r
		JOIN Categories c ON c.Id = r.CategoryId
		JOIN Departments d ON c.DepartmentId = d.Id
		WHERE r.Id = @ReportId)

	IF(@employeeDepartmentId <> @reportDepartmentId)
	BEGIN
		THROW 51000, 'Employee doesn''t belong to the appropriate department!', 1
	END

	UPDATE Reports
	SET EmployeeId = @EmployeeId
	WHERE Id = @ReportId
END

GO
EXEC usp_AssignEmployeeToReport 30, 1
EXEC usp_AssignEmployeeToReport 17, 2