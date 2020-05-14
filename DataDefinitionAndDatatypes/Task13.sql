USE master

CREATE DATABASE Movies;
GO

USE Movies;

CREATE TABLE Directors(
	Id INT NOT NULL PRIMARY KEY,
	DirectorName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
);

CREATE TABLE Genres(
	Id INT NOT NULL PRIMARY KEY,
	GenreName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
);

CREATE TABLE Categories(
	Id INT NOT NULL PRIMARY KEY,
	CategoryName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
);

CREATE TABLE Movies
(
	Id INT NOT NULL PRIMARY KEY,
	Title NVARCHAR(255) NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Directors(Id),
	CopyrightYear INT,
	Length NVARCHAR(50),
	GenreId INT FOREIGN KEY REFERENCES Genres(Id),
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
	Rating INT,
	Notes NVARCHAR(MAX)
);

INSERT INTO Directors(Id, DirectorName)
VALUES(1, 'Director One'),
		(2, 'Director Two'),
		(3, 'Director Three'),
		(4, 'Director Four'),
		(5, 'Director Five');

INSERT INTO Genres(Id, GenreName)
VALUES(1, 'GenreName One'),
		(2, 'GenreName Two'),
		(3, 'GenreName Three'),
		(4, 'GenreName Four'),
		(5, 'GenreName Five');

INSERT INTO Categories(Id, CategoryName)
VALUES(1, 'CategoryName One'),
		(2, 'CategoryName Two'),
		(3, 'CategoryName Three'),
		(4, 'CategoryName Four'),
		(5, 'CategoryName Five');

INSERT INTO Movies(Id, Title, DirectorId, GenreId, CategoryId)
VALUES(1, 'Title One', 5, 1, 5),
		(2, 'Title Two', 4, 2, 4),
		(3, 'Title Three', 3, 3, 3),
		(4, 'Title Four', 2, 4, 2),
		(5, 'Title Five', 1, 5, 1);