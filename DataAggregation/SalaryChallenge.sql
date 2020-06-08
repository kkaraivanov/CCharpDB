WITH temp AS(SELECT e.DepartmentId Id, AVG(e.Salary) salary FROM Employees e
	GROUP BY e.DepartmentId)
SELECT TOP(10) e.FirstName, e.LastName, e.DepartmentID FROM Employees e, temp t
WHERE e.DepartmentID = t.Id AND e.Salary > t.salary
ORDER BY e.DepartmentID