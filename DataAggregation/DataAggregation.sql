/***************************************************************************
	Exercises: Data Aggregation
***************************************************************************/

/* Problem 1.	Records’ Count */
SELECT COUNT(*) FROM WizzardDeposits;

/* Problem 2.	Longest Magic Wand */
SELECT MAX(w.MagicWandSize) LongestMagicWand FROM WizzardDeposits w;

/* Problem 3.	Longest Magic Wand per Deposit Groups */
SELECT DepositGroup, MAX(MagicWandSize) LongestMagicWand FROM WizzardDeposits w
GROUP BY DepositGroup;

/* Smallest Deposit Group per Magic Wand Size */
SELECT TOP 2 DepositGroup FROM WizzardDeposits
GROUP BY DepositGroup
ORDER BY AVG(MagicWandSize);

/* Problem 5.	Deposits Sum */
SELECT DepositGroup, SUM(DepositAmount) TotalSum FROM WizzardDeposits
GROUP BY DepositGroup;

/* Problem 6.	Deposits Sum for Ollivander Family */
SELECT DepositGroup, SUM(DepositAmount) TotalSum FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup;

/* Problem 7.	Deposits Filter */
SELECT DepositGroup, SUM(DepositAmount) TotalSum FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup
HAVING SUM(DepositAmount) < 150000
ORDER BY TotalSum DESC;

/* Deposit Charge */
SELECT DepositGroup, MagicWandCreator, MIN(DepositCharge) MinDepositCharge FROM WizzardDeposits
GROUP BY DepositGroup, MagicWandCreator
ORDER BY MagicWandCreator, DepositGroup;

/* Problem 9.	Age Groups */
SELECT 
	CASE
		WHEN Age BETWEEN 0 AND 10 THEN '[0-10]'
		WHEN Age BETWEEN 11 AND 20 THEN '[11-20]'
		WHEN Age BETWEEN 21 AND 30 THEN '[21-30]'
		WHEN Age BETWEEN 31 AND 40 THEN '[31-40]'
		WHEN Age BETWEEN 41 AND 50 THEN '[41-50]'
		WHEN Age BETWEEN 51 AND 60 THEN '[51-60]'
		WHEN Age >= 61 THEN '[61+]'
	END AgeGroup,
	COUNT(*) WizardCount FROM WizzardDeposits
	GROUP BY CASE
		WHEN Age BETWEEN 0 AND 10 THEN '[0-10]'
		WHEN Age BETWEEN 11 AND 20 THEN '[11-20]'
		WHEN Age BETWEEN 21 AND 30 THEN '[21-30]'
		WHEN Age BETWEEN 31 AND 40 THEN '[31-40]'
		WHEN Age BETWEEN 41 AND 50 THEN '[41-50]'
		WHEN Age BETWEEN 51 AND 60 THEN '[51-60]'
		WHEN Age >= 61 THEN '[61+]'
	END;

/* Problem 10.	First Letter */
SELECT DISTINCT SUBSTRING(FirstName, 1, 1) FirstLetter FROM WizzardDeposits
WHERE DepositGroup = 'Troll Chest'
GROUP BY FirstName
ORDER BY FirstLetter;

/* Problem 11.	Average Interest */
SELECT DepositGroup, IsDepositExpired, AVG(DepositInterest) AverageInterest FROM WizzardDeposits
WHERE DepositStartDate > '01/01/1985'
GROUP BY DepositGroup, IsDepositExpired
ORDER BY DepositGroup DESC, IsDepositExpired ASC;

/* Problem 12.	* Rich Wizard, Poor Wizard */
SELECT SUM(rt.diff) SumDifference FROM (SELECT DepositAmount - (
	SELECT DepositAmount FROM WizzardDeposits
	WHERE Id = WizDeposits.Id + 1) diff FROM WizzardDeposits WizDeposits) rt;

/* Problem 13.	Departments Total Salaries */
SELECT DepartmentID, SUM(Salary) TotalSalary FROM Employees
GROUP BY DepartmentID
ORDER BY DepartmentID;

/* Problem 14.	Employees Minimum Salaries */
SELECT DepartmentID, MIN(Salary) MinimumSalary FROM Employees
WHERE DepartmentID IN(2, 5, 7) AND HireDate > '01/01/2000'
GROUP BY DepartmentID;

/* Problem 15.	Employees Average Salaries */
SELECT * INTO NewTable FROM Employees
WHERE Salary > 30000;

DELETE FROM NewTable
WHERE ManagerID = 42;

UPDATE NewTable
SET Salary += 5000
WHERE DepartmentID = 1;

SELECT DepartmentID, AVG(Salary) AverageSalary FROM NewTable
GROUP BY DepartmentID;

DROP TABLE NewTable;

/* Problem 16.	Employees Maximum Salaries */
SELECT DepartmentID, MAX(Salary) MaxSalary FROM Employees
GROUP BY DepartmentID
HAVING MAX(Salary) NOT BETWEEN 30000 AND 70000;

/* Problem 17.	Employees Count Salaries */
SELECT COUNT(EmployeeID) Count FROM Employees
WHERE ManagerID IS NULL
GROUP BY ManagerID;

/* Problem 18.	*3rd Highest Salary */
SELECT DepartmentID,
(SELECT DISTINCT e.Salary FROM Employees e
	WHERE e.DepartmentID = emp.DepartmentID ORDER BY e.Salary DESC
	OFFSET 2 ROWS FETCH NEXT 1 ROWS ONLY) ThirdHighestSalary
FROM Employees emp
WHERE (SELECT DISTINCT e.Salary FROM Employees e
	WHERE e.DepartmentID = emp.DepartmentID ORDER BY e.Salary DESC
	OFFSET 2 ROWS FETCH NEXT 1 ROWS ONLY) IS NOT NULL
GROUP BY DepartmentID;

/* Problem 19.	**Salary Challenge */
WITH temp AS(SELECT e.DepartmentId Id, AVG(e.Salary) salary FROM Employees e
	GROUP BY e.DepartmentId)
SELECT TOP(10) e.FirstName, e.LastName, e.DepartmentID FROM Employees e, temp t
WHERE e.DepartmentID = t.Id AND e.Salary > t.salary
ORDER BY e.DepartmentID