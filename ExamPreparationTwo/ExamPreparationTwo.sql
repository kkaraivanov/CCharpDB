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
