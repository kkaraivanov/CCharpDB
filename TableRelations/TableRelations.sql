/*****************************************************************
		Exercises: Table Relations
*****************************************************************/

/* Problem 1.	One-To-One Relationship */
CREATE TABLE Passports(
	PassportID INT PRIMARY KEY NOT NULL,
	PassportNumber NVARCHAR(10) NOT NULL
);

CREATE TABLE Persons(
	PersonId INT IDENTITY(1, 1) NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	Salary MONEY,
	PassportID INT,
	PRIMARY KEY(PersonId),
	CONSTRAINT FK_Passport FOREIGN KEY (PassportID)
	REFERENCES Passports(PassportID)
);

INSERT INTO Passports(PassportID, PassportNumber)
	VALUES(101, 'N34FG21B'), 
		(102, 'K65LO4R7'), 
		(103, 'ZE657QP2');

INSERT INTO Persons(FirstName, Salary, PassportID)
	VALUES('Roberto', 43300.00,	102),
		('Tom', 56100.00,	103),
		('Yana', 60200.00,	101);


/* Problem 2.	One-To-Many Relationship */
CREATE TABLE Manufacturers(
	ManufacturerID INT IDENTITY(1, 1) NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	EstablishedOn DATETIME2,
	PRIMARY KEY (ManufacturerID)
);

 CREATE TABLE Models(
	ModelId INT PRIMARY KEY NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	ManufacturerID INT,
	CONSTRAINT FK_Manufacturer FOREIGN KEY (ManufacturerID)
	REFERENCES Manufacturers(ManufacturerID));

INSERT INTO Manufacturers([Name], EstablishedOn)
	VALUES('BMW', '07/03/1916'),
		('Tesla', '01/01/2003'),
		('Lada', '01/05/1966');

INSERT INTO Models(ModelId,[Name], ManufacturerID)
	VALUES('101', 'X1',	1),
		('102', 'i6',	1),
		('103', 'Model S',	2),
		('104', 'Model X',	2),
		('105', 'Model 3',	2),
		('106', 'Nova',	3);

/* Problem 3.	Many-To-Many Relationship */
CREATE TABLE Students(
	StudentID INT IDENTITY(1, 1) PRIMARY KEY,
	[Name] NVARCHAR(50) NOT NULL);

CREATE TABLE Exams(
	ExamID INT IDENTITY(101, 1) PRIMARY KEY,
	[Name] NVARCHAR(50) NOT NULL);

CREATE TABLE StudentsExams(
	StudentID INT FOREIGN KEY(StudentID) REFERENCES Students(StudentID),
	ExamID INT FOREIGN KEY(ExamID) REFERENCES Exams(ExamID),
	PRIMARY KEY (StudentID, ExamID));

INSERT INTO Students([Name])
    VALUES
        ('Mila'),
        ('Toni'),
        ('Ron');
 
INSERT INTO Exams([Name])
    VALUES
        ('Spring MVC'),
        ('Neo4j'),
        ('Oracle 11g');
 
INSERT INTO StudentsExams
    VALUES  
        (1, 101),
        (1, 102),
        (2, 101),
        (3, 103),
        (2, 102),
        (2, 103);

/* Problem 4.	Self-Referencing  */
CREATE TABLE Teachers(
	TeacherId INT IDENTITY(101, 1) PRIMARY KEY,
	[NAME] NVARCHAR(50) NOT NULL,
	ManagerID INT FOREIGN KEY REFERENCES Teachers (TeacherId));

INSERT INTO Teachers([NAME], ManagerID)
	VALUES('John', NULL),
		('Maya', 106),
		('Silvia', 106),
		('Ted',	105),
		('Mark', 101),
		('Greta', 101);

/* Problem 5.	Online Store Database */
CREATE TABLE Cities(
	CityID INT, 
	[Name] VARCHAR(50),
	CONSTRAINT PK_Cities PRIMARY KEY (CityID));

CREATE TABLE Customers(
	CustomerID INT,
	[Name] VARCHAR(50),
	Birthday DATE,
	CityID INT, 
	CONSTRAINT PK_Customers PRIMARY KEY (CustomerID),
	CONSTRAINT FK_Customers_Cities FOREIGN KEY (CityID)
	REFERENCES Cities(CityID));

CREATE TABLE Orders(
	OrderID INT,
	CustomerID INT,
	CONSTRAINT PK_Orders PRIMARY KEY(OrderID),
	CONSTRAINT FK_Orders_Customers FOREIGN KEY(CustomerID)
	REFERENCES Customers(customerID));


CREATE TABLE ItemTypes(
	ItemTypeID INT,
	[Name] VARCHAR(50),
	CONSTRAINT PK_ItemTypes PRIMARY KEY(ItemTypeID));

CREATE TABLE Items(
	ItemID INT, 
	[Name] VARCHAR(50),
	ItemTypeID INT,
	CONSTRAINT PK_Items PRIMARY KEY(ItemID),
	CONSTRAINT FP_Items_ItemTypes FOREIGN KEY(ItemTypeID)
	REFERENCES ItemTypes(ItemTypeID));

CREATE TABLE OrderItems(
	OrderID INT, 
	ItemID INT,
	CONSTRAINT PK_OrderItems PRIMARY KEY(OrderID, ItemID),
	CONSTRAINT FK_OI_Orders FOREIGN KEY (OrderID)
	REFERENCES Orders(OrderID),
	CONSTRAINT FK_OI_Items FOREIGN KEY(ItemID)
	REFERENCES Items(ItemID));

/* Problem 6.	University Database */
