/*CREATE DATABASE School
GO
USE School
GO
*/

/* Section 1. DDL */
CREATE TABLE Students(
    Id INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(30) NOT NULL,
    MiddleName NVARCHAR(25),
    LastName NVARCHAR(30) NOT NULL,
    Age INT NOT NULL CHECK(Age BETWEEN 5 AND 100),
    [Address] NVARCHAR(50),
    Phone CHAR(10)
)

CREATE TABLE Subjects(
    Id INT PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(20) NOT NULL,
    Lessons INT NOT NULL CHECK(Lessons > 0)
)

CREATE TABLE StudentsSubjects(
    Id INT PRIMARY KEY IDENTITY,
    StudentId INT NOT NULL FOREIGN KEY REFERENCES Students(Id),
    SubjectId INT NOT NULL FOREIGN KEY REFERENCES Subjects(Id),
    Grade DECIMAL(16,2) NOT NULL CHECK(Grade BETWEEN 2 AND 6)
)

CREATE TABLE Exams(
    Id INT PRIMARY KEY IDENTITY,
    [Date] DATETIME2,
    SubjectId INT NOT NULL FOREIGN KEY REFERENCES Subjects(Id)
)

CREATE TABLE StudentsExams(
    StudentId INT NOT NULL FOREIGN KEY REFERENCES Students(Id),
    ExamId INT NOT NULL FOREIGN KEY REFERENCES Exams(Id),
    Grade DECIMAL(16,2) NOT NULL CHECK(Grade BETWEEN 2 AND 6),
    PRIMARY KEY(StudentId, ExamId)
)

CREATE TABLE Teachers(
    Id INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(20) NOT NULL,
    LastName NVARCHAR(20) NOT NULL,
    [Address] NVARCHAR(20) NOT NULL,
    Phone CHAR(10),
    SubjectId INT NOT NULL FOREIGN KEY REFERENCES Subjects(Id)
)

CREATE TABLE StudentsTeachers(
    StudentId INT NOT NULL FOREIGN KEY REFERENCES Students(Id),
    TeacherId INT NOT NULL FOREIGN KEY REFERENCES Teachers(Id),
    PRIMARY KEY(StudentId, TeacherId)
)

/* 2. Insert */
INSERT INTO Teachers(FirstName, LastName, [Address], Phone,	SubjectId) VALUES
('Ruthanne', 'Bamb', '84948 Mesta Junction', 3105500146, 6),
('Gerrard', 'Lowin', '370 Talisman Plaza', 3324874824, 2),
('Merrile', 'Lambdin', '81 Dahle Plaza', 4373065154,	5),
('Bert', 'Ivie', '2 Gateway Circle', 4409584510, 4)

INSERT INTO Subjects(Name, Lessons) VALUES
('Geometry', 12),
('Health', 10),
('Drama', 7),
('Sports', 9)

/* 3. Update */
UPDATE StudentsSubjects
SET Grade = 6.0
WHERE Grade >= 5.50 AND SubjectId IN(1, 2)

/* 4. Delete */
DELETE FROM StudentsTeachers
WHERE TeacherId IN((SELECT Id FROM Teachers WHERE Phone LIKE '%72%'))
DELETE FROM Teachers
WHERE Phone LIKE '%72%'

/* 5. Teen Students */
SELECT FirstName, LastName, Age FROM Students
WHERE Age >= 12
ORDER BY FirstName, LastName

/* 6. Students Teachers */
SELECT s.FirstName,	s.LastName, COUNT(st.TeacherId)	TeachersCount FROM Students s
JOIN StudentsTeachers st ON s.Id = st.StudentId
GROUP BY s.FirstName,	s.LastName

/* 7. Students to Go */
SELECT s.FirstName + ' ' + LastName [Full Name] FROM Students s
LEFT JOIN StudentsExams se ON s.Id = se.StudentId
WHERE se.StudentId IS NULL
ORDER BY [Full Name] ASC

/* 8. Top Students */ 
SELECT TOP(10)
	s.FirstName,
	s.LastName,
	CAST(AVG(se.Grade) AS DECIMAL(3, 2)) [Grade] 
FROM Students s
JOIN StudentsExams se ON s.Id = se.StudentId
GROUP BY s.FirstName, s.LastName
ORDER BY [Grade] DESC, FirstName ASC, LastName ASC

/* 9. Not So In The Studying */
SELECT CONCAT(s.FirstName, ' ', ISNULL(s.MiddleName + ' ', ''), s.LastName) [Full Name]
FROM Students s
LEFT JOIN StudentsSubjects ss ON s.Id = ss.StudentId
WHERE ss.SubjectId IS NULL
ORDER BY [Full Name]

/* 10. Average Grade per Subject */
SELECT s.Name, AVG(ss.Grade) [AverageGrade] FROM Subjects s
JOIN StudentsSubjects ss ON s.Id = ss.SubjectId
GROUP BY s.Id, s.Name
ORDER BY s.Id

/* 11. Exam Grades */
CREATE FUNCTION udf_ExamGradesToUpdate(@studentId INT, @grade DECIMAL(16, 2))
RETURNS VARCHAR(200) AS
BEGIN
	DECLARE @findBigGrade DECIMAL(16, 2) = @grade + 0.50
	DECLARE @findStudentId INT = (SELECT TOP(1) StudentId FROM StudentsExams WHERE StudentId = @studentId)
	DECLARE @result VARCHAR(200)
	DECLARE @findCount INT = 
		(SELECT COUNT(*) FROM StudentsExams
		WHERE StudentId = @studentId AND Grade >= @grade AND Grade <= @findBigGrade)
	DECLARE @studentFirstName NVARCHAR(30) = 
		(SELECT TOP(1) FirstName FROM Students WHERE Id = @studentId)
	
	IF(@findStudentId IS NULL)
	BEGIN
		SET @result = 'The student with provided id does not exist in the school!'
		RETURN @result
	END

	IF(@grade > 6.00)
	BEGIN
		SET @result = 'Grade cannot be above 6.00!'
		RETURN @result
	END

	SET @result = CONCAT('You have to update ', @findCount, ' grades for the student ', @studentFirstName)
	RETURN @result
END

SELECT dbo.udf_ExamGradesToUpdate(12, 6.20)
SELECT dbo.udf_ExamGradesToUpdate(12, 5.50)
SELECT dbo.udf_ExamGradesToUpdate(121, 5.50)

/* 12. Exclude from school */
CREATE OR ALTER PROC usp_ExcludeFromSchool(@StudentId INT) AS
BEGIN
	DECLARE @studentExist INT = 
		(SELECT Id FROM Students WHERE Id = @StudentId)

	IF(@studentExist IS NULL)
	BEGIN
		RAISERROR('This school has no student with the provided id!', 16, 1)
		RETURN
	END

	DELETE FROM StudentsSubjects
	WHERE StudentId = @StudentId

	DELETE FROM StudentsExams
	WHERE StudentId = @StudentId

	DELETE FROM StudentsTeachers
	WHERE StudentId = @StudentId

	DELETE FROM Students
	WHERE Id = @StudentId
END

EXEC usp_ExcludeFromSchool 1
SELECT COUNT(*) FROM Students

EXEC usp_ExcludeFromSchool 301