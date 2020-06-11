/****************************************************************
Database Programmability and Transactions
****************************************************************/

/* Problem 1. Employees with Salary Above 35000 */
CREATE PROCEDURE usp_GetEmployeesSalaryAbove35000
AS
SELECT FirstName, LastName FROM Employees
WHERE Salary > 35000;

/* Problem 2. Employees with Salary Above Number */
CREATE PROCEDURE usp_GetEmployeesSalaryAboveNumber(@Value DECIMAL(18,4))
AS
SELECT FirstName, LastName FROM Employees
WHERE Salary >= @Value;
GO
EXEC usp_GetEmployeesSalaryAboveNumber 48100;

/* Problem 3. Town Names Starting With */
CREATE PROCEDURE usp_GetTownsStartingWith(@Value NVARCHAR(50))
AS
SELECT Name Town FROM Towns
WHERE LEFT(Name, LEN(@Value)) = @value;

/* Problem 4. Employees from Town */
CREATE PROCEDURE usp_GetEmployeesFromTown(@Value NVARCHAR(50))
AS
SELECT e.FirstName, e.LastName FROM Employees e
JOIN Addresses a ON e.AddressID = a.AddressID
JOIN Towns t ON a.TownID = t.TownID
WHERE t.Name = @Value;

/* Problem 5. Salary Level Function */
CREATE  FUNCTION dbo.ufn_GetSalaryLevel(@Salary DECIMAL(18,4)) 
RETURNS VARCHAR(10) AS 
BEGIN 
	DECLARE @SalaryLevel VARCHAR(10)
	IF(@Salary < 30000)
	BEGIN 
	 SET @SalaryLevel = 'Low'
	END
	ELSE IF(@Salary >= 30000 AND @Salary <= 50000)
	BEGIN
	 SET @SalaryLevel = 'Average'
	END
	ELSE 
	BEGIN 
	 SET @SalaryLevel = 'High'
	END
RETURN @SalaryLevel
END;

/* Problem 6. Employees by Salary Level */
CREATE PROC usp_EmployeesBySalaryLevel(@SalaryLevel VARCHAR(10))
AS 
SELECT FirstName,LastName FROM Employees 
WHERE dbo.ufn_GetSalaryLevel(Salary) = @SalaryLevel;

/* Problem 7. Define Function */
CREATE FUNCTION dbo.ufn_IsWordComprised(@setOfLetters VARCHAR(50), @word VARCHAR(50))
RETURNS BIT
AS
BEGIN
	DECLARE @indx INT
    DECLARE @len INT
    DECLARE @currentChar CHAR(1)
    SET @indx = 1
    SET @len= LEN(@word)

    WHILE @indx <= @len
    BEGIN
        SET @currentChar = SUBSTRING(@word, @indx, 1)
        IF (CHARINDEX(@currentChar, @setOfLetters)) = 0
			RETURN 0

        SET @indx = @indx + 1
    END
    RETURN 1
END;

/* Problem 8. * Delete Employees and Departments */
CREATE PROCEDURE usp_DeleteEmployeesFromDepartment (@departmentId INT)
AS
ALTER TABLE Departments
ALTER COLUMN ManagerID INT NULL

DECLARE @tempTable TABLE(Id int)

INSERT INTO @tempTable
SELECT EmployeeID FROM Employees
WHERE DepartmentID = @departmentId

UPDATE Employees
SET ManagerID = NULL
WHERE ManagerID IN(SELECT Id FROM @tempTable)

UPDATE Departments
SET ManagerID = NULL
WHERE ManagerID IN(SELECT Id FROM @tempTable)

DELETE FROM EmployeesProjects
WHERE EmployeeID IN(SELECT Id FROM @tempTable)

DELETE FROM Employees
WHERE EmployeeID IN(SELECT Id FROM @tempTable)

DELETE FROM Departments
WHERE DepartmentID = @departmentId

SELECT COUNT(*) Employees FROM Employees e
JOIN Departments d ON d.DepartmentID = e.DepartmentID
WHERE e.DepartmentID = @departmentId

/* Problem 9. Find Full Name */
CREATE PROCEDURE usp_GetHoldersFullName
AS
SELECT (FirstName + ' ' + LastName) [Full Name] FROM AccountHolders

/* Problem 10. People with Balance Higher Than */
CREATE PROCEDURE usp_GetHoldersWithBalanceHigherThan(@Value MONEY)
AS
WITH balance AS (SELECT AccountHolderId, SUM(Balance) num FROM Accounts
GROUP BY AccountHolderId)
SELECT a.FirstName, a.LastName FROM AccountHolders a--, balance b
JOIN balance b ON b.AccountHolderId = a.Id
WHERE b.num > @Value
ORDER BY a.FirstName, a.LastName

/* Problem 11. Future Value Function */
CREATE FUNCTION dbo.ufn_CalculateFutureValue(@sum DECIMAL(18,4), @yearlnterestRate FLOAT, @years INT)
RETURNS DECIMAL(18,4)
AS
BEGIN
-- FV = I × POWER((1+R), T) 
-- I – Initial sum
-- R – Yearly interest rate
-- T – Number of years
	DECLARE @returnValue DECIMAL(18,4)
	SET @returnValue = @sum * POWER((1 + @yearlnterestRate), @years)

	RETURN @returnValue
END;

/* Problem 12. Calculating Interest */
CREATE PROCEDURE dbo.usp_CalculateFutureValueForAccount(@acountId INT, @interestRate FLOAT)
AS
	SELECT b.Id [Account Id], 
		a.FirstName [First Name],
		a.LastName [Last Name],
		b.Balance [Current Balance],
		dbo.ufn_CalculateFutureValue(b.Balance, @interestRate, 5) [Balance in 5 years]
	FROM AccountHolders a
	JOIN Accounts b ON a.Id = b.AccountHolderId
	WHERE b.Id = @acountId

EXEC dbo.usp_CalculateFutureValueForAccount 1, 0.10

/* Problem 13. *Scalar Function: Cash in User Games Odd Rows */
CREATE FUNCTION dbo.ufn_CashInUsersGames(@gameName NVARCHAR(MAX))
  RETURNS TABLE 
AS
  RETURN  SELECT SUM(Cash) [SumCash]
			FROM (SELECT ug.Cash, ROW_NUMBER() OVER (ORDER BY ug.Cash DESC ) RowNum FROM UsersGames ug
			      JOIN Games g ON ug.GameId = g.Id
			      WHERE g.Name = @gameName) CashList
			WHERE RowNum % 2 = 1

/* Problem 14. Create Table Logs */
CREATE TABLE Logs (
  LogId INT PRIMARY KEY IDENTITY,
  AccountId INT,
  OldSum MONEY,
  NewSum MONEY
);

CREATE TRIGGER InsertNewEntryIntoLogs ON Accounts FOR UPDATE
AS
BEGIN
  INSERT INTO Logs
  VALUES (
    (SELECT Id FROM INSERTED),
    (SELECT Balance FROM DELETED),
    (SELECT Balance FROM INSERTED)
  )
END;

/* Problem 15. Create Table Emails */
CREATE TABLE NotificationEmails(
	Id INT PRIMARY KEY IDENTITY,
	Recipient NVARCHAR(250), 
	[Subject] NVARCHAR(250),
	Body NVARCHAR(250)
)

CREATE TRIGGER NotificationEmailsLogs ON Logs FOR INSERT
AS
BEGIN
	DECLARE @recipient INT = (SELECT AccountId FROM INSERTED)
	DECLARE @oldSum DECIMAL(18, 4) = (SELECT OldSum FROM INSERTED)
	DECLARE @newSum DECIMAL(18, 4) = (SELECT NewSum FROM INSERTED)

	INSERT INTO NotificationEmails(Recipient, [Subject], Body)
	VALUES
	(
	    @recipient,
	    'Balance change for account: ' + CAST(@recipient AS NVARCHAR(15)),
	    'On ' + CAST(GETDATE() AS NVARCHAR(50)) + ' your balance was changed from ' +
	    CAST(@oldSum AS NVARCHAR(30)) + ' to ' +
	    CAST(@newSum AS NVARCHAR(50)) + '.'
	)
END

/* Problem 16. Deposit Money */
CREATE PROC usp_DepositMoney(@accountId INT, @moneyAmount DECIMAL(18, 4)) AS
BEGIN
	DECLARE @account INT = 
		(SELECT Id FROM dbo.Accounts
		WHERE id = @accountId)

	IF(@moneyAmount < 0 OR @moneyAmount IS NULL)
	BEGIN
		RETURN
	END

	IF(@account IS NULL)
	BEGIN
		RETURN
	END

	UPDATE dbo.Accounts
	SET
		dbo.Accounts.Balance += @moneyAmount
		WHERE dbo.Accounts.Id = @accountId
END

/* Problem 17. Withdraw Money */
CREATE PROC usp_WithdrawMoney(@accountId INT, @moneyAmount DECIMAL(18, 4)) AS
BEGIN
	DECLARE @account INT = 
		(SELECT Id FROM dbo.Accounts
		WHERE id = @accountId)

	IF(@moneyAmount < 0 OR @moneyAmount IS NULL)
	BEGIN
		RETURN
	END

	IF(@account IS NULL)
	BEGIN
		RETURN
	END

	UPDATE dbo.Accounts
	SET
		dbo.Accounts.Balance -= @moneyAmount
		WHERE dbo.Accounts.Id = @accountId
END

/* Problem 18. Money Transfer */
CREATE PROC usp_TransferMoney(@senderId INT, @receiverId INT, @amount DECIMAL(18, 4)) AS
BEGIN
	DECLARE @sender INT = (SELECT Id FROM dbo.Accounts
		WHERE id = @senderId)
	DECLARE @receiver INT = (SELECT Id FROM dbo.Accounts
		WHERE id = @receiverId)

	IF(@amount < 0 OR @amount IS NULL)
	BEGIN
		RETURN
	END

	IF(@sender IS NULL OR @receiver IS NULL)
	BEGIN
		RETURN
	END

	EXEC usp_WithdrawMoney @senderId, @amount
	EXEC usp_DepositMoney @receiverId, @amount
END

/* Problem 19. Trigger */
select* from UsersGames

select* from Items