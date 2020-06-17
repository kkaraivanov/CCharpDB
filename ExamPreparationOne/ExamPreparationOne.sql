CREATE DATABASE Airport
GO
USE Airport
GO

/* Section 1. DDL  */
CREATE TABLE Planes(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(30) NOT NULL,
	Seats INT NOT NULL,
	[Range] INT NOT NULL
)

CREATE TABLE Flights(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	DepartureTime DATETIME2,
	ArrivalTime DATETIME2,
	Origin VARCHAR(50) NOT NULL,
	Destination VARCHAR(50) NOT NULL,
	PlaneId INT NOT NULL FOREIGN KEY REFERENCES Planes(Id)
)

CREATE TABLE Passengers(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR(30) NOT NULL,
	LastName VARCHAR(30) NOT NULL,
	Age INT NOT NULL,
	[Address] VARCHAR(30) NOT NULL,
	PassportId VARCHAR(11) NOT NULL
)

CREATE TABLE LuggageTypes(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[Type] VARCHAR(30) NOT NULL
)

CREATE TABLE Luggages(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	LuggageTypeId INT NOT NULL FOREIGN KEY REFERENCES LuggageTypes(Id),
	PassengerId INT NOT NULL FOREIGN KEY REFERENCES Passengers(Id)
)

CREATE TABLE Tickets(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	PassengerId INT NOT NULL FOREIGN KEY REFERENCES Passengers(Id),
	FlightId INT NOT NULL FOREIGN KEY REFERENCES Flights(Id),
	LuggageId INT NOT NULL FOREIGN KEY REFERENCES Luggages(Id),
	Price DECIMAL(6, 2) NOT NULL
)

/* 2.	Insert */
INSERT INTO Planes(Name, Seats, Range) VALUES
('Airbus 336', 112, 5132),
('Airbus 330', 432, 5325),
('Boeing 369', 231, 2355),
('Stelt 297', 254, 2143),
('Boeing 338', 165, 5111),
('Airbus 558', 387, 1342),
('Boeing 128', 345, 5541)

INSERT INTO LuggageTypes(Type) VALUES
('Crossbody Bag'),
('School Backpack'),
('Shoulder Bag')

/* 3.	Update */
UPDATE Tickets
SET Price += Price * 0.13
WHERE FlightId = (SELECT Id FROM Flights
				WHERE Destination = 'Carlsbad')

/* 4.	Delete */
DELETE FROM Tickets
WHERE FlightId = (SELECT Id FROM Flights WHERE Destination = 'Ayn Halagim')
DELETE FROM Flights
WHERE Destination = 'Ayn Halagim'

/* 5.	The "Tr" Planes */
SELECT * FROM Planes
WHERE Name LIKE '%tr%'
ORDER BY Id, Name, Seats, Range

/* 6.	Flight Profits */
SELECT FlightId [FlightId], SUM(Price) [Price] FROM Tickets
GROUP BY FlightId
ORDER BY Price DESC, FlightId ASC

/* 7.	Passenger Trips */
SELECT FirstName + ' ' + LastName [Full Name],
	f.Origin [Origin],
	f.Destination [Destination]
FROM Passengers p
JOIN Tickets t ON p.Id = t.PassengerId
JOIN Flights f ON t.FlightId = f.Id
ORDER BY [Full Name], Origin, Destination

/* 8.	Non Adventures People */
SELECT p.FirstName [First Name],
	p.LastName [Last Name],
	p.Age [Age]
FROM Passengers p
LEFT JOIN Tickets t ON p.Id = t.PassengerId
WHERE t.Id IS NULL
ORDER BY Age DESC, [First Name] ASC, [Last Name] ASC

/* 9.	Full Info */
SELECT p.FirstName + ' ' + p.LastName [Full Name],
	pl.Name [Plane Name],
	f.Origin + ' - ' + f.Destination [Trip],
	lt.Type [Luggage Type]
FROM Passengers p
LEFT JOIN Tickets t ON p.Id = t.PassengerId
JOIN Flights f ON t.FlightId = f.Id
JOIN Planes pl ON f.PlaneId = pl.Id
RIGHT JOIN Luggages l ON t.LuggageId = l.Id
JOIN LuggageTypes lt ON l.LuggageTypeId = lt.Id
WHERE t.Id IS NOT NULL
ORDER BY [Full Name] ASC, [Plane Name] ASC, Origin ASC, Destination ASC, [Luggage Type] ASC

/* 10.	PSP */
SELECT p.Name, p.Seats, COUNT(t.PassengerId) [Passengers Count] FROM Planes p
FULL JOIN Flights f ON p.Id = f.PlaneId
FULL JOIN Tickets t ON f.Id = t.FlightId
GROUP BY p.Name, p.Seats
ORDER BY [Passengers Count] DESC, p.Name ASC, p.Seats ASC

/* 11.	Vacation */
CREATE FUNCTION udf_CalculateTickets(@origin VARCHAR(50), @destination VARCHAR(50), @peopleCount INT)
RETURNS VARCHAR(100)
AS
BEGIN
	DECLARE @returnResult VARCHAR(100)

	IF(@peopleCount <= 0)
	BEGIN
		SET @returnResult = 'Invalid people count!'
		RETURN @returnResult
	END

	DECLARE @flightId INT = (
		SELECT TOP(1) Id FROM Flights
		WHERE @origin = Origin AND @destination = Destination)

	IF(@flightId IS NULL)
	BEGIN
		SET @returnResult = 'Invalid flight!'
		RETURN @returnResult
	END

	DECLARE @ticketPrice DECIMAL(16, 2) = (
		SELECT TOP(1) Price FROM Tickets
		WHERE @flightId = FlightId)

	DECLARE @totalPrice DECIMAL(16, 2) = @ticketPrice * @peopleCount
	
	SET @returnResult = CONCAT('Total price ', @totalPrice)

	RETURN @returnResult
END

/* 12.	Wrong Data */
CREATE PROC usp_CancelFlights
AS
BEGIN
	UPDATE Flights
	SET DepartureTime = NULL, ArrivalTime = NULL
	WHERE ArrivalTime > DepartureTime
END