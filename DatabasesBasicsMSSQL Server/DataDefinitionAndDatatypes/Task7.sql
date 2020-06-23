CREATE TABLE People(
	Id INT UNIQUE IDENTITY NOT NULL,
	[Name] NVARCHAR (200) NOT NULL,
	Picture VARBINARY(max) NULL,
	Height  DECIMAL(3,2) NULL,
	[Weight] DECIMAL(5,2) NULL,
	Gender NVARCHAR(1) NOT NULL CHECK(Gender in ('F', 'M')),
	Birthdate DATE NOT NULL,
	Biography NVARCHAR(255) NULL,
);
ALTER TABLE People
ADD PRIMARY KEY(Id);

ALTER TABLE People
ADD CONSTRAINT CH_PictureSize CHECK(DATALENGTH(Picture) <= 2 * 1024 * 1024);

INSERT INTO People([Name], Gender,Birthdate)
VALUES ('Pesho', 'M', '01-25-1999'),
		('Gosho', 'M', '10-12-2009'),
		('Mimi', 'F', '06-25-1002'),
		('Petia', 'F', '01-29-1992'),
		('Ivan', 'M', '11-11-1997');