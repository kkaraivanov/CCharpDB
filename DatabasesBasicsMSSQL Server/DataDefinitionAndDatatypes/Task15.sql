USE master

CREATE DATABASE Hotel;
GO

USE Hotel;

/* Create tables
	Employees (Id, FirstName, LastName, Title, Notes)
	Customers (AccountNumber, FirstName, LastName, PhoneNumber, EmergencyName, EmergencyNumber, Notes)
	RoomStatus (RoomStatus, Notes)
	RoomTypes (RoomType, Notes)
	BedTypes (BedType, Notes)
	Rooms (RoomNumber, RoomType, BedType, Rate, RoomStatus, Notes)
	Payments (Id, EmployeeId, PaymentDate, AccountNumber, FirstDateOccupied, LastDateOccupied, TotalDays, AmountCharged, TaxRate, TaxAmount, PaymentTotal, Notes)
	Occupancies (Id, EmployeeId, DateOccupied, AccountNumber, RoomNumber, RateApplied, PhoneCharge, Notes)
*/

CREATE TABLE Employees(
	Id INT PRIMARY KEY NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Title NVARCHAR(255) NOT NULL,
	Notes NVARCHAR(MAX)
);

CREATE TABLE Customers(
	AccountNumber INT PRIMARY KEY NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	PhoneNumber VARCHAR(50),
	EmergencyName NVARCHAR(50) NOT NULL,
	EmergencyNumber INT NOT NULL,
	Notes NVARCHAR(50)
);

CREATE TABLE RoomStatus(
	RoomStatus NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(MAX)
);

CREATE TABLE RoomTypes(
	RoomType NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(MAX)
);

CREATE TABLE BedTypes(
	BedType NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(MAX)
);

CREATE TABLE Rooms(
	RoomNumber INT PRIMARY KEY NOT NULL,
	RoomType NVARCHAR(50) NOT NULL,
	BedType NVARCHAR(50) NOT NULL,
	Rate DECIMAL(10, 2) NOT NULL,
	RoomStatus NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
);

CREATE TABLE Payments(
	Id INT PRIMARY KEY NOT NULL,
	EmployeeId INT NOT NULL,
	PaymentDate DATE NOT NULL,
	AccountNumber INT NOT NULL,
	FirstDateOccupied DATE NOT NULL,
	LastDateOccupied DATE NOT NULL,
	TotalDays INT NOT NULL,
	AmountCharged DECIMAL(10, 2) NOT NULL,
	TaxRate DECIMAL(10, 2) NOT NULL,
	TaxAmount DECIMAL(10, 2) NOT NULL,
	PaymentTotal DECIMAL(10, 2) NOT NULL,
	Notes NVARCHAR(MAX)
);

ALTER TABLE Payments
ADD CONSTRAINT CK_TotalDays CHECK(DATEDIFF(DAY, FirstDateOccupied, LastDateOccupied) = TotalDays);

ALTER TABLE Payments
ADD CONSTRAINT CK_TaxAmount CHECK(TaxAmount = TotalDays * TaxRate);

CREATE TABLE Occupancies(
	Id INT PRIMARY KEY NOT NULL,
	EmployeeId INT NOT NULL,
	DateOccupied DATE NOT NULL,
	AccountNumber INT NOT NULL,
	RoomNumber INT NOT NULL,
	RateApplied DECIMAL(10, 2),
	PhoneCharge VARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
);

/* Insert data in tables */

INSERT INTO Employees(Id, FirstName, LastName, Title)
VALUES(1, 'First', 'Employee', 'Manager'),
		(2, 'Second', 'Employee', 'Manager'),
		(3, 'Third', 'Employee', 'Manager');

INSERT INTO Customers(AccountNumber, FirstName, LastName, EmergencyName, EmergencyNumber)
VALUES(1, 'First', 'Customer', 'Em1',  11111),
		(2, 'Second', 'Customer', 'Em2', 22222),
		(3, 'Third', 'Customer', 'Em3', 33333);

INSERT INTO RoomStatus(RoomStatus)
VALUES('Free'),
		('In use'),
		('Reserved');

INSERT INTO RoomTypes(RoomType)
VALUES('Luxory'),
		('Casual'),
		( 'Misery');

INSERT INTO BedTypes(BedType)
VALUES('Single'),
		('Double'),
		('King');

INSERT INTO Rooms(RoomNumber, RoomType, BedType, Rate, RoomStatus)
VALUES(1, 'Luxory', 'King', 100, 'Reserved'),
		(2, 'Casual', 'Double', 50, 'In use'),
		(3, 'Misery', 'Single', 10, 'Free');

INSERT INTO Payments(Id, EmployeeId, PaymentDate, AccountNumber, FirstDateOccupied, LastDateOccupied, TotalDays, AmountCharged, TaxRate, TaxAmount, PaymentTotal)
VALUES(1, 2, '12-23-2020', 1, '12-23-2020', '12-24-2020', 1, 75, 75, 75, 75),
		(2, 2, '12-23-2020', 1, '12-23-2020', '12-24-2020', 1, 75, 75, 75, 75),
		(3, 2, '12-23-2020', 1, '12-23-2020', '12-24-2020', 1, 75, 75, 75, 75);

INSERT INTO Occupancies(Id, EmployeeId, DateOccupied, AccountNumber, RoomNumber, PhoneCharge)
VALUES(1, 2, '08-24-2020', 3, 1, '088 88 888 888'),
		(2, 2, '08-24-2020', 3, 1, '088 88 888 888'),
		(3, 2, '08-24-2020', 3, 1, '088 88 888 888');