/*******************************************************************************
--	Exercises: Built-in Functions
*******************************************************************************/

/* Problem 1.	Find Names of All Employees by First Name */
SELECT [FirstName], [LastName] FROM Employees
WHERE [FirstName] LIKE('SA%');

/* Problem 2.	  Find Names of All employees by Last Name */
SELECT [FirstName], [LastName] FROM Employees
WHERE [LastName] LIKE('%ei%');

/* Problem 3.	Find First Names of All Employees */
SELECT [FirstName] FROM Employees
WHERE [DepartmentID] = 3 OR [DepartmentID] = 10
AND YEAR([HireDate]) BETWEEN 1995 AND 2005;

/* Problem 4.	Find All Employees Except Engineers */
SELECT [FirstName], [LastName] FROM Employees
WHERE [JobTitle] NOT LIKE('%engineer%');

/* Problem 5.	Find Towns with Name Length */
SELECT [Name] FROM Towns
WHERE LEN([Name]) = 5 OR LEN([Name]) = 6
ORDER BY [Name];

/* Problem 6.	 Find Towns Starting With */
SELECT [TownID], [Name] FROM Towns
WHERE [Name] LIKE '[MKBE]%'
ORDER BY [Name];

/* Problem 7.	 Find Towns Not Starting With */
SELECT [TownID], [Name] FROM Towns
WHERE [Name] NOT LIKE '[RBD]%'
ORDER BY [Name];

/* Problem 8.	Create View Employees Hired After 2000 Year */
CREATE VIEW [V_EmployeesHiredAfter2000] AS
SELECT [FirstName], [LastName] FROM Employees
WHERE YEAR([HireDate]) > '2000';

/* Problem 9.	Length of Last Name */
SELECT [FirstName], [LastName] FROM Employees
WHERE LEN([LastName]) = 5;

/* Problem 10.	Rank Employees by Salary */
SELECT [EmployeeID], [FirstName], [LastName], [Salary], 
	DENSE_RANK() OVER(PARTITION BY[Salary] ORDER BY [EmployeeID]) AS [Rank]
FROM Employees
WHERE [Salary] BETWEEN 10000 AND 50000
ORDER BY [Salary] DESC;

/* Problem 11.	Find All Employees with Rank 2 * */
SELECT * From
	(SELECT [EmployeeID], [FirstName], [LastName], [Salary], 
		DENSE_RANK() OVER(PARTITION BY[Salary] ORDER BY [EmployeeID]) AS [Rank]
	FROM Employees
	WHERE [Salary] BETWEEN 10000 AND 50000) AS temp
WHERE [Rank] = 2
ORDER BY [Salary] DESC;

/******************************************************************************/
Use Geography
GO
/******************************************************************************/

/* Problem 12.	Countries Holding ‘A’ 3 or More Times */
SELECT [CountryName], [IsoCode]
FROM Countries
WHERE LEN(CountryName) - LEN(replace([CountryName],'a','')) > 2
ORDER BY IsoCode;

/* Problem 13.	 Mix of Peak and River Names */
SELECT peak.PeakName, river.RiverName, LOWER(LEFT(PeakName, LEN(PeakName) - 1) + RiverName) AS [Mix]
FROM Peaks peak, Rivers river
WHERE RIGHT(PeakName, 1) = LEFT(RiverName, 1)
ORDER BY [Mix];

/******************************************************************************/
Use Diablo
GO
/******************************************************************************/

/* Problem 14.	Games from 2011 and 2012 year */
SELECT TOP (50) [Name], FORMAT([Start], 'yyyy-MM-dd') AS [Start] FROM Games
WHERE DATEPART(YEAR, [Start]) BETWEEN 2011 and 2012
ORDER BY [Start], [Name]

/* Problem 15.	 User Email Providers */
SELECT [Username], RIGHT(Email, LEN(Email) - CHARINDEX('@', Email)) 
AS [Email Provaider] FROM Users
ORDER BY [Email Provaider], [Username];

/* Problem 16.	 Get Users with IPAdress Like Pattern * |***.1^.^.***| */
SELECT [Username], [IpAddress] FROM Users
WHERE [IpAddress] LIKE '[0-9][0-9][0-9].1%.%.[0-9][0-9][0-9]'
ORDER BY [Username];

/* Problem 17.	 Show All Games with Duration and Part of the Day */
SELECT 
[Name] AS Game,
CASE
	WHEN DATEPART(HOUR, Start) BETWEEN 0 AND 11 THEN 'Morning'
	WHEN DATEPART(HOUR, Start) BETWEEN 12 AND 17 THEN 'Afternoon'
	WHEN DATEPART(HOUR, Start) BETWEEN 18 AND 24 THEN 'Evening'
END AS [Part of the Day],
CASE
	WHEN Duration <= 3 THEN 'Extra Short'
	WHEN Duration BETWEEN 4 AND 6 THEN 'Short'
	WHEN Duration > 6 THEN 'Long'
	WHEN Duration IS NULL THEN 'Extra Long'
END AS [Duration]
FROM Games
ORDER BY [Name], [Duration], [Part of the Day];

/******************************************************************************/
CREATE DATABASE Orders;
GO

USE Orders;
GO
CREATE TABLE Orders
(
Id INT NOT NULL,
ProductName VARCHAR(50) NOT NULL,
OrderDate DATETIME NOT NULL
CONSTRAINT PK_Orders PRIMARY KEY (Id)
)

INSERT INTO Orders (Id, ProductName, OrderDate) VALUES (1, 'Butter', '20160919');
INSERT INTO Orders (Id, ProductName, OrderDate) VALUES (2, 'Milk', '20160930');
INSERT INTO Orders (Id, ProductName, OrderDate) VALUES (3, 'Cheese', '20160904');
INSERT INTO Orders (Id, ProductName, OrderDate) VALUES (4, 'Bread', '20151220');
INSERT INTO Orders (Id, ProductName, OrderDate) VALUES (5, 'Tomatoes', '20150101');
INSERT INTO Orders (Id, ProductName, OrderDate) VALUES (6, 'Tomatoe2', '20150201');
INSERT INTO Orders (Id, ProductName, OrderDate) VALUES (7, 'Tomatoess', '20150401');
INSERT INTO Orders (Id, ProductName, OrderDate) VALUES (8, 'Tomatoe3', '20150128');
INSERT INTO Orders (Id, ProductName, OrderDate) VALUES (9, 'Tomatoe4', '20150628');
INSERT INTO Orders (Id, ProductName, OrderDate) VALUES (10, 'Tomatoe44s', '20150630');
INSERT INTO Orders (Id, ProductName, OrderDate) VALUES (11, 'Tomatoefggs', '20150228');
INSERT INTO Orders (Id, ProductName, OrderDate) VALUES (12, 'Tomatoesytu', '20160228');
INSERT INTO Orders (Id, ProductName, OrderDate) VALUES (13, 'Toyymatddoehys', '20151231');
INSERT INTO Orders (Id, ProductName, OrderDate) VALUES (14, 'Tom443atoes', '20151215');
INSERT INTO Orders (Id, ProductName, OrderDate) VALUES (15, 'Tomat65434foe23gfhgsPep', '20151004');
/******************************************************************************/

/* Problem 18.	 Orders Table */
SELECT orders.ProductName, orders.OrderDate,
	DATEADD(DAY, 3, orders.OrderDate) AS [Pay Due],
	DATEADD(MONTH, 1, orders.OrderDate) AS [Deliver Due]
FROM Orders orders
