USE master

CREATE DATABASE SoftUni;
GO

USE SoftUni;

/* Create tables 
•	Towns (Id, Name)
•	Addresses (Id, AddressText, TownId)
•	Departments (Id, Name)
•	Employees (Id, FirstName, MiddleName, LastName, JobTitle, DepartmentId, HireDate, Salary, AddressId)
*/

CREATE TABLE Towns(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	[Name] NVARCHAR(50) NOT NULL
);

CREATE TABLE Addresses(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	AddressText NVARCHAR(100) NOT NULL,
	TownId INT FOREIGN KEY REFERENCES Towns(Id) NOT NULL
);

CREATE TABLE Departments(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	[Name] NVARCHAR(50) NOT NULL
);

CREATE TABLE Employees(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	MiddleName NVARCHAR(50),
	LastName NVARCHAR(50),
	JobTitle NVARCHAR(100) NOT NULL,
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id) NOT NULL,
	HireDate DATE,
	Salary DECIMAL(10, 2) NOT NULL,
	AddressId INT FOREIGN KEY REFERENCES Addresses(Id)
);

/* Backup the database SoftUni, drop SoftUni and restore from beckup file*/
BACKUP DATABASE SoftUni TO DISK = 'C:\softuni-backup.bak';

USE master
DROP DATABASE SoftUni;

RESTORE DATABASE SoftUni FROM DISK = 'C:\softuni-backup.bak';
USE SoftUni
/*************************************************************************/

/* Insert data in tables from SoftUni base												 */

INSERT INTO Towns(Name)
VALUES('Sofia'),
	('Plovdiv'),
	('Varna'),
	('Burgas');

INSERT INTO Departments(Name)
VALUES('Engineering'),
	('Sales'),
	('Marketing'),
	('Software Development'),
	('Quality Assurance');

INSERT INTO Employees(FirstName, MiddleName, LastName, JobTitle, DepartmentId, HireDate, Salary)
VALUES('Ivan', 'Ivanov', 'Ivanov', '.NET Developer', 4, CONVERT(DATE, '02/03/2004', 103), 3500.00),
	('Petar', 'Petrov', 'Petrov', 'Senior Engineer', 1, CONVERT(DATE, '02/03/2004', 103), 4000.00),
	('Stoian', 'Stoianov', 'Stoianov', 'Intern', 5, CONVERT(DATE, '28/08/2016', 103), 525.25),
	('Georgi', 'Georgiev', 'Georgiev', 'CEO', 2, CONVERT(DATE, '09/12/2007', 103), 3000.00),
	('Blagoi', 'Blagoev', 'Blagoev', 'Intern', 3, CONVERT(DATE, '28/08/2016', 103), 599.88);

/* Problem 19 Basic Select All Fields */

SELECT *
FROM Towns;

SELECT *
FROM Departments;

SELECT *
FROM Employees;

/* Problem 20 Basic Select All Fields and Order Them */

SELECT *
FROM Towns
ORDER BY Name ASC;

SELECT *
FROM Departments
ORDER BY Name ASC;

SELECT *
FROM Employees
ORDER BY Salary DESC;

/* Problem 21 Basic Select Some Fields */

SELECT [Name]
FROM Towns
ORDER BY [Name] ASC;

SELECT [Name]
FROM Departments
ORDER BY [Name] ASC;

SELECT [FirstName],
       [LastName],
       [JobTitle],
       [Salary]
FROM Employees
ORDER BY Salary DESC;

/* Problem 22 Increase Employees Salary */

UPDATE Employees
SET Salary *= 1.10;

SELECT [Salary]
FROM Employees;

/* Problem 23 Decrease Tax Rate */

USE Hotel;

/* ALTER TABLE Payments
DROP CONSTRAINT [CK_TaxAmount]; */

UPDATE Payments
SET TaxRate = TaxRate - (TaxRate * 0.03);

SELECT TaxRate
FROM Payments;

/* Problem 24 Delete All Records */

TRUNCATE TABLE Occupancies;

/************************ COMPLETE **************************/