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