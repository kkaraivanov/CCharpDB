/**********************************************************
	Additional Exercises
**********************************************************/

/* Problem 1.	Number of Users for Email Provider */
SELECT RIGHT(Email, LEN(Email) - CHARINDEX('@', Email)) [Email Provider],
	COUNT(Email) [Number Of Users]
	FROM Users
GROUP BY RIGHT(Email, LEN(Email) - CHARINDEX('@', Email))
ORDER BY [Number Of Users] DESC, [Email Provider] ASC

/* Problem 2.	All User in Games */
SELECT g.Name [Game], gt.Name [Game Type], u.Username, ug.Level, ug.Cash, c.Name [Character] FROM Users u
RIGHT JOIN UsersGames ug ON u.Id = ug.UserId
JOIN Games g ON ug.GameId = g.Id
JOIN GameTypes gt ON g.GameTypeId = gt.Id
JOIN Characters c ON ug.CharacterId = c.Id
ORDER BY ug.Level DESC, u.Username ASC, [GAME] ASC

/* Problem 3.	Users in Games with Their Items */
SELECT u.Username, g.Name [Game], COUNT(ugi.UserGameId) [Items Count], SUM(i.Price) [Items Price] FROM Users u
RIGHT JOIN UsersGames ug ON u.Id = ug.UserId
JOIN Games g ON ug.GameId = g.Id
JOIN GameTypes gt ON g.GameTypeId = gt.Id
JOIN UserGameItems ugi ON ug.Id = ugi.UserGameId
JOIN Items i ON ugi.ItemId = i.Id
GROUP BY g.Name, u.Username
HAVING COUNT(ugi.UserGameId) >= 10
ORDER BY COUNT(ugi.ItemId) DESC, SUM(i.Price) DESC, u.Username

/* Problem 4.	* User in Games with Their Statistics */
WITH sumItems AS 
	(SELECT ug.UserId,  ug.GameId, 
	SUM(s.Strength) [Strength], 
	SUM(s.Defence) [Defence], 
	SUM(s.Speed) [Speed], 
	SUM(s.Mind) [Mind], 
	SUM(s.Luck) [Luck] FROM UsersGames ug
	JOIN UserGameItems ugi ON ugi.UserGameId = ug.Id
	JOIN Items i ON ugi.ItemId = i.Id
	JOIN [Statistics] s ON s.Id = i.StatisticId
	GROUP BY ug.UserId, ug.GameId)
SELECT DISTINCT u.Username,
	   g.[Name] [Game],
	   MAX(c.[Name]) [Character],
	   MAX(charStats.Strength) + MAX(gtStats.Strength) + MAX(sumIts.Strength) [Strength],
	   MAX(charStats.Defence) + MAX(gtStats.Defence) + MAX(sumIts.Defence) [Defence],
	   MAX(charStats.Speed) + MAX(gtStats.Speed) + MAX(sumIts.Speed) [Speed],
	   MAX(charStats.Mind) + MAX(gtStats.Mind) + MAX(sumIts.Mind) [Mind],
	   MAX(charStats.Luck) + MAX(gtStats.Luck) + MAX(sumIts.Luck) [Luck]
FROM UsersGames ug
JOIN Users u ON u.Id = ug.UserId
JOIN Games g ON g.Id = ug.GameId
JOIN UserGameItems ugi ON ugi.UserGameId = ug.Id
JOIN Items i ON ugi.ItemId = i.Id
JOIN Characters c ON c.Id = ug.CharacterId
JOIN GameTypes gt ON gt.Id = g.GameTypeId
JOIN [Statistics] charStats ON charStats.Id = c.StatisticId
JOIN [Statistics] gtStats ON gtStats.Id = gt.BonusStatsId
JOIN sumItems sumIts ON sumIts.UserId = u.Id AND sumIts.GameId = g.Id
GROUP BY u.Username, g.[Name]
ORDER BY Strength DESC, Defence DESC, Speed DESC, Mind DESC, Luck DESC

/* Problem 5.	All Items with Greater than Average Statistics */
SELECT i.Name, i.Price, i.MinLevel,
	AVG(s.Strength) [Strength],
	AVG(s.Defence) [Defence],
	AVG(s.Speed) [Speed],
	AVG(s.Luck) [Luck],
	AVG(s.Mind) [Mind] FROM UsersGames ug
JOIN UserGameItems ugi ON ugi.UserGameId = ug.Id
JOIN Items i ON ugi.ItemId = i.Id
RIGHT JOIN [Statistics] s ON s.Id = i.StatisticId
WHERE i.Name IS NOT NULL AND 
s.Speed > (SELECT AVG(Speed) from [Statistics]) AND
Luck > (SELECT AVG(Luck) from [Statistics]) AND
Mind > (SELECT AVG(Mind) from [Statistics])
GROUP BY i.Name, i.Price, i.MinLevel
ORDER BY i.Name

/* Problem 6.	Display All Items with Information about Forbidden Game Type */
SELECT i.Name [Item], i.Price [Price], i.MinLevel [MinLevel], gt.Name [Forbidden Game Type] FROM Items i
FULL JOIN GameTypeForbiddenItems gtf ON i.Id = gtf.ItemId
FULL JOIN GameTypes gt ON gtf.GameTypeId = gt.Id
ORDER BY [Forbidden Game Type] DESC, [Item]

/* Problem 7.	Buy Items for User in Game */
SELECT u.Username [Username], g.Name [Name], ug.Cash [Cash], i.Name [Item Name] FROM Users u
	JOIN UsersGames ug ON u.Id = ug.UserId
	JOIN Games g ON ug.GameId = g.Id
	JOIN UserGameItems ugi ON ug.Id = ugi.UserGameId
	JOIN Items i ON ugi.ItemId = i.Id
WHERE g.Name = 'Edinburgh'
ORDER BY [Item Name]

INSERT INTO UserGameItems (ItemId, UserGameId)
VALUES (
	(SELECT Id FROM Items
		WHERE Name = 'Hellfire Amulet'
	),
	(SELECT ug.Id FROM UsersGames ug
		JOIN Users u ON ug.UserId = u.Id
		JOIN Games g ON ug.GameId = g.Id
		WHERE u.Username = 'Alex' AND g.Name = 'Edinburgh'
	)
)
UPDATE UsersGames
SET Cash = ( SELECT ug.Cash - i.Price FROM UsersGames ug 
JOIN UserGameItems ugi ON ug.Id = ugi.UserGameId
JOIN Items i ON ugi.ItemId = i.Id
JOIN Users u ON ug.UserId = u.Id
JOIN Games g ON ug.GameId = g.Id
	WHERE 
		u.Username = 'Alex' AND
		g.Name = 'Edinburgh' AND 
		i.Name IN ('Blackguard',
			'Bottomless Potion of Amplification',
			'Eye of Etlich (Diablo III)',
			'Gem of Efficacious Toxin',
			'Golden Gorget of Leoric ',
			'Hellfire Amulet')
)
WHERE Id = (SELECT ug.Id FROM UsersGames ug
WHERE ug.GameId = (SELECT Id FROM Games WHERE Name = 'Edinburgh')
AND ug.UserId = (SELECT Id FROM Users WHERE Username = 'Alex'))

/* Problem 8.	Peaks and Mountains */
SELECT p.PeakName, m.MountainRange [Mountain], p.Elevation FROM Peaks p
JOIN Mountains m ON p.MountainId = m.Id
ORDER BY p.Elevation DESC, p.PeakName

/* Problem 9.	Peaks with Their Mountain, Country and Continent */
SELECT p.PeakName, m.MountainRange [Mountain], c.CountryName, con.ContinentName FROM Peaks p
JOIN Mountains m ON p.MountainId = m.Id
JOIN MountainsCountries mc ON m.Id = mc.MountainId
JOIN Countries c ON mc.CountryCode = c.CountryCode
JOIN Continents con ON c.ContinentCode = con.ContinentCode
ORDER BY p.PeakName, c.CountryName

/* Problem 10.	Rivers by Country */
SELECT c.CountryName, con.ContinentName,
	ISNULL(COUNT(r.Id), 0) [RiversCount],
	ISNULL(SUM(r.Length), 0) [TotalLength] FROM Countries c
JOIN Continents con ON c.ContinentCode = con.ContinentCode
LEFT JOIN CountriesRivers cr ON c.CountryCode = cr.CountryCode
LEFT JOIN Rivers r ON cr.RiverId = r.Id
GROUP BY c.CountryName, con.ContinentName
ORDER BY [RiversCount] DESC, [TotalLength] DESC, c.CountryName ASC

/* Problem 11.	Count of Countries by Currency */
SELECT cu.CurrencyCode [CurrencyCode],
	cu.Description [Currency],
	COUNT(c.CurrencyCode) [NumberOfCountries] FROM Countries c 
RIGHT JOIN Currencies cu ON cu.CurrencyCode = c.CurrencyCode
GROUP BY cu.CurrencyCode, cu.Description
ORDER BY [NumberOfCountries] DESC, [Currency] ASC

/* Problem 12.	Population and Area by Continent */
SELECT * FROM Continents;

SELECT co.ContinentName,
	SUM(c.AreaInSqKm) [CountriesArea],
	SUM(CAST(c.[Population] AS FLOAT)) [CountriesPopulation]
	FROM Countries c
JOIN Continents co ON c.ContinentCode = co.ContinentCode
GROUP BY co.ContinentName
ORDER BY CountriesPopulation DESC

/* Problem 13.	Monasteries by Country */
CREATE TABLE Monasteries(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	[Name] NVARCHAR(50),
	CountryCode CHAR(2) FOREIGN KEY REFERENCES Countries(CountryCode)
	)

INSERT INTO Monasteries(Name, CountryCode) VALUES
('Rila Monastery “St. Ivan of Rila”', 'BG'), 
('Bachkovo Monastery “Virgin Mary”', 'BG'),
('Troyan Monastery “Holy Mother''s Assumption”', 'BG'),
('Kopan Monastery', 'NP'),
('Thrangu Tashi Yangtse Monastery', 'NP'),
('Shechen Tennyi Dargyeling Monastery', 'NP'),
('Benchen Monastery', 'NP'),
('Southern Shaolin Monastery', 'CN'),
('Dabei Monastery', 'CN'),
('Wa Sau Toi', 'CN'),
('Lhunshigyia Monastery', 'CN'),
('Rakya Monastery', 'CN'),
('Monasteries of Meteora', 'GR'),
('The Holy Monastery of Stavronikita', 'GR'),
('Taung Kalat Monastery', 'MM'),
('Pa-Auk Forest Monastery', 'MM'),
('Taktsang Palphug Monastery', 'BT'),
('Sümela Monastery', 'TR')

ALTER TABLE Countries
ADD IsDeleted BIT NULL
CONSTRAINT Countries_IsDeleted DEFAULT(0);
GO
UPDATE Countries
SET IsDeleted = 0;
GO
ALTER TABLE Countries
ALTER COLUMN IsDeleted BIT NOT NULL;
GO

UPDATE Countries
SET IsDeleted = 1
WHERE CountryCode IN (
	SELECT c.CountryCode
	FROM Countries c
	JOIN CountriesRivers cr ON c.CountryCode = cr.CountryCode
	JOIN Rivers r ON cr.RiverId = r.Id
	GROUP BY c.CountryCode
	HAVING COUNT(r.Id) > 3)

SELECT m.Name [Monastery], c.CountryName [Country] FROM Monasteries m
JOIN Countries c ON m.CountryCode = c.CountryCode
WHERE c.IsDeleted = 0
ORDER BY Monastery;

/* Problem 14.	Monasteries by Continents and Countries */
UPDATE Countries
SET CountryName = 'Burma'
WHERE CountryName = 'Myanmar'

INSERT INTO Monasteries(Name, CountryCode) 
SELECT 'Hanga Abbey', CountryCode FROM Countries WHERE CountryName = 'Tanzania'
INSERT INTO Monasteries(Name, CountryCode) 
SELECT 'Myin-Tin-Daik', CountryCode FROM Countries WHERE CountryName = 'Myanmar'

SELECT co.ContinentName [ContinentName],
	c.CountryName [CountryName],
	COUNT(m.Id) [MonasteriesCount]
	FROM Continents co
JOIN Countries c ON co.ContinentCode = c.ContinentCode
LEFT JOIN Monasteries m ON c.CountryCode = m.CountryCode
WHERE c.IsDeleted = 0
GROUP BY co.ContinentName, c.CountryName
ORDER BY MonasteriesCount DESC, CountryName ASC