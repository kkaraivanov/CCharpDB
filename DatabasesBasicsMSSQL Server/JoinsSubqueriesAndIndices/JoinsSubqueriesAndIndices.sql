/*****************************************************************************************
	Joins, Subqueries, CTE and Indices
*****************************************************************************************/

/* Problem 1.	Employee Address */
SELECT TOP(5) EmployeeId, JobTitle, emp.AddressId, adr.AddressText FROM Employees AS emp
INNER JOIN Addresses AS adr ON emp.AddressID = adr.AddressID
ORDER BY emp.AddressID ASC;

/* Problem 2.	Addresses with Towns */
SELECT TOP(50) FirstName, LastName, t.Name AS Town, a.AddressText FROM Employees e
JOIN Addresses a ON e.AddressID = a.AddressID
JOIN Towns t ON a.TownID = t.TownID
ORDER BY e.FirstName ASC, e.LastName;

/* Problem 3.	Sales Employee */
SELECT EmployeeID, FirstName, LastName, d.Name DepartmentName FROM Employees e
JOIN Departments d ON e.DepartmentID = d.DepartmentID
WHERE d.Name = 'Sales'
ORDER BY e.EmployeeID ASC;

/* Problem 4.	Employee Departments */
SELECT TOP(5) EmployeeID, FirstName, Salary, d.Name DepartmentName FROM Employees e
JOIN Departments d ON e.DepartmentID = d.DepartmentID
WHERE Salary > 15000
ORDER BY e.DepartmentID ASC;

/* Problem 5.	Employees Without Project */
SELECT TOP(3) EmployeeID, FirstName FROM Employees e
WHERE EmployeeID NOT IN(SELECT ep.EmployeeID FROM EmployeesProjects ep)
ORDER BY EmployeeID ASC;

/* Problem 6.	Employees Hired After */
SELECT FirstName, LastName, HireDate, d.Name DeptName FROM Employees e
JOIN Departments d ON e.DepartmentID = d.DepartmentID
WHERE HireDate > '1.1.1999' AND d.Name = 'Sales' OR d.Name = 'Finance'
ORDER BY HireDate ASC;

/* Problem 7.	Employees with Project */
SELECT TOP(5) e.EmployeeID, FirstName, p.Name ProjectName FROM Employees e
JOIN EmployeesProjects ep ON e.EmployeeID = ep.EmployeeID
JOIN Projects p ON ep.ProjectID = p.ProjectID
WHERE p.StartDate > ISDATE('13.08.2002') AND p.EndDate IS NULL
ORDER BY e.EmployeeID ASC;

/* Problem 8.	Employee 24 */
SELECT e.EmployeeID, FirstName, p.Name ProjectName FROM Employees e
INNER JOIN EmployeesProjects ep ON e.EmployeeID = ep.EmployeeID
LEFT JOIN Projects p ON ep.ProjectID = p.ProjectID AND p.StartDate < '20050101'
WHERE e.EmployeeID = 24;

/* Problem 9.	Employee Manager */
SELECT e.EmployeeID, e.FirstName, e.ManagerID, ej.FirstName ManagerName FROM Employees e
JOIN Employees ej ON e.ManagerID = ej.EmployeeID
WHERE e.ManagerID = 3 OR e.ManagerID = 7
ORDER BY e.EmployeeID ASC

/* Problem 10.	Employee Summary */
SELECT TOP(50) e.EmployeeID, 
	e.FirstName + ' ' + e.LastName EmployeeName, 
	ej.FirstName + ' ' +  ej.LastName ManagerName, 
	d.Name DepartmentName FROM Employees e
JOIN Employees ej ON e.ManagerID = ej.EmployeeID
JOIN Departments d ON e.DepartmentID = d.DepartmentID
ORDER BY e.EmployeeID ASC;

/* Problem 11.	Min Average Salary */
SELECT TOP (1) AVG(Salary) MinAverageSalary FROM Employees
GROUP BY DepartmentID
ORDER BY AVG(Salary);

/****************************************************************************************/
USE Geography
GO
/****************************************************************************************/

/* Problem 12.	Highest Peaks in Bulgaria */
SELECT mc.CountryCode CountryCode, m.MountainRange, p.PeakName, p.Elevation FROM Peaks p
JOIN MountainsCountries mc ON p.MountainId = mc.MountainId
JOIN Mountains m ON p.MountainId = m.Id
WHERE mc.CountryCode = 'BG' AND p.Elevation > 2835
ORDER BY p.Elevation DESC;

/* Problem 13.	Count Mountain Ranges */
SELECT c.CountryCode, COUNT(m.MountainRange) MountainRanges FROM Mountains m
JOIN MountainsCountries mc ON m.Id = mc.MountainId
JOIN Countries c ON mc.CountryCode = c.CountryCode
WHERE c.CountryName IN('United States', 'Russia', 'Bulgaria')
GROUP BY c.CountryCode;

/* Problem 14.	Countries with Rivers */
SELECT c.CountryName, r.RiverName FROM Countries c
FULL JOIN CountriesRivers cr ON c.CountryCode = cr.CountryCode
FULL JOIN Rivers r ON cr.RiverId = r.Id
WHERE c.ContinentCode = 'AF'
ORDER BY c.CountryName
OFFSET 0 ROWS
FETCH NEXT 5 ROWS ONLY;

/* Problem 15.	*Continents and Currencies */
SELECT rc.ContinentCode, rc.CurrencyCode, 
	rc.Count FROM (SELECT c.ContinentCode, c.CurrencyCode, COUNT(c.CurrencyCode) [Count], 
		DENSE_RANK() OVER (PARTITION BY c.ContinentCode ORDER BY COUNT(c.CurrencyCode) DESC) [rank] 
		FROM Countries c
		GROUP BY c.ContinentCode, c.CurrencyCode) rc
WHERE rc.rank = 1 and rc.Count > 1;

/* Problem 16.	Countries without any Mountains */
SELECT COUNT(*) Count FROM Countries c
FULL JOIN MountainsCountries m ON c.CountryCode = m.CountryCode
WHERE m.MountainId IS NULL;

/* Problem 17.	Highest Peak and Longest River by Country */
SELECT c.CountryName, MAX(p.Elevation) HighestPeakElevation, MAX(r.Length) LongestRiverLength FROM Countries c
LEFT JOIN MountainsCountries mc ON c.CountryCode = mc.CountryCode
LEFT JOIN Peaks p ON p.MountainId = mc.MountainId
LEFT JOIN CountriesRivers cr ON c.CountryCode = cr.CountryCode
LEFT JOIN Rivers r ON cr.RiverId = r.Id
GROUP BY c.CountryName
ORDER BY HighestPeakElevation DESC, LongestRiverLength DESC, c.CountryName
OFFSET 0 ROWS
FETCH NEXT 5 ROWS ONLY;

/* Problem 18.	* Highest Peak Name and Elevation by Country */
WITH chp AS (SELECT c.CountryName, p.PeakName, p.Elevation, m.MountainRange,
   ROW_NUMBER() OVER ( PARTITION BY c.CountryName ORDER BY p.Elevation DESC ) rn FROM Countries c
LEFT JOIN CountriesRivers AS cr ON c.CountryCode = cr.CountryCode
LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
LEFT JOIN Mountains AS m ON mc.MountainId = m.Id
LEFT JOIN Peaks p ON p.MountainId = m.Id)

SELECT TOP 5 chp.CountryName [Country],
  ISNULL(chp.PeakName, '(no highest peak)') [Highest Peak Name],
  ISNULL(chp.Elevation, 0) [Highest Peak Elevation],
  CASE WHEN chp.PeakName IS NOT NULL
    THEN chp.MountainRange
  ELSE '(no mountain)' END [Mountain] FROM chp
WHERE rn = 1 
ORDER BY chp.CountryName ASC, chp.PeakName ASC