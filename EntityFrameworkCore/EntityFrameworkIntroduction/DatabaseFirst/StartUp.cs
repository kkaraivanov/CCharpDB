namespace SoftUni
{
    using System;
    using System.Linq;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using SoftUni.Data;
    using SoftUni.Models;

    public class StartUp
    {
        static void Main(string[] args)
        {
            var dbContext = new SoftUniContext();

            Console.WriteLine(GetDepartmentsWithMoreThan5Employees(dbContext));
        }

        // 3.	Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees;
            var employeeInfo = new StringBuilder();

            foreach (var employee in employees)
            {
                var currentEmployee = $"{employee.FirstName} " +
                                      $"{employee.LastName} " +
                                      $"{employee.MiddleName} " +
                                      $"{employee.JobTitle} " +
                                      $"{employee.Salary:f2}";
                employeeInfo.AppendLine(currentEmployee);
            }

            return employeeInfo.ToString().TrimEnd();
        }

        // 4.	Employees with Salary Over 50 000
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employeeInfo = new StringBuilder();
            var employees = context.Employees
                .Where(x => x.Salary > 50000)
                .OrderBy(x => x.FirstName);

            foreach (var employee in employees)
            {
                var currentEmployee = $"{employee.FirstName} - {employee.Salary:f2}"; //{firstName} - {salary}.
                employeeInfo.AppendLine(currentEmployee);
            }

            return employeeInfo.ToString().TrimEnd();
        }

        // 5.	Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employeeInfo = new StringBuilder();
            var employees = context.Employees
                .Where(x => x.Department.Name == "Research and Development")
                .OrderBy(employee => employee.Salary)
                .ThenByDescending(employee => employee.FirstName);

            foreach (var employee in employees)
            {
                //Gigi Matthew from Research and Development - $40900.00.
                var currentEmployee = $"{employee.FirstName} " +
                                      $"{employee.LastName} from Research and Development " +
                                      $"- ${employee.Salary:f2}";
                employeeInfo.AppendLine(currentEmployee);
            }

            return employeeInfo.ToString().TrimEnd();
        }

        // 6.	Adding a New Address and Updating Employee
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var employeeInfo = new StringBuilder();

            var address = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };
            var empoyee = context.Employees.First(x => x.LastName == "Nakov");
            empoyee.Address = address;
            context.SaveChanges();

            context.Employees
                .OrderByDescending(x => x.Address.AddressId)
                .Take(10)
                .Select(x => x.Address.AddressText)
                .ToList()
                .ForEach(x => employeeInfo.AppendLine(x));

            return employeeInfo.ToString().TrimEnd();
        }

        // 7.	Employees and Projects
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employeeInfo = new StringBuilder();
            var employees = context.Employees
                .Where(x => x.EmployeesProjects
                    .Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .Take(10)
                .Select(employe => new
                {
                    employe.FirstName,
                    employe.LastName,
                    ManagerFirstName = employe.Manager.FirstName,
                    ManagerLastName = employe.Manager.LastName,
                    Projects = employe.EmployeesProjects.Select(ep => ep.Project)
                })
                .ToList();

            foreach (var employee in employees)
            {
                employeeInfo
                    .AppendLine(
                    $"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}{Environment.NewLine}" +
                    $"{String.Join(Environment.NewLine, employee.Projects.Select(p => $"--{p.Name} - {p.StartDate.ToString("M/d/yyyy h:mm:ss tt")} - {(p.EndDate == null ? "not finished" : p.EndDate.ToString())}"))}");
            }

            return employeeInfo.ToString().TrimEnd();
        }

        // 8.	Addresses by Town
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var employeeInfo = new StringBuilder();
            var addresses =
                context.Addresses
                    .Include(x => x.Employees)
                    .Include(x => x.Town)
                    .OrderByDescending(x => x.Employees.Count)
                    .ThenBy(x => x.Town.Name)
                    .ThenBy(x => x.AddressText)
                    .Take(10)
                    .ToList();

            addresses.ForEach(x =>
                employeeInfo.AppendLine($"{x.AddressText}, {x.Town.Name} - {x.Employees.Count} employees"));

            return employeeInfo.ToString().TrimEnd();
        }

        // 9.	Employee 147
        public static string GetEmployee147(SoftUniContext context)
        {
            var employeeInfo = new StringBuilder();
            var employee = context.Employees
                .First(x => x.EmployeeId == 147);

            employeeInfo.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            foreach (var project in employee.EmployeesProjects
                .OrderBy(x => x.Project.Name))
            {
                employeeInfo.AppendLine($"{project.Project.Name}");
            }

            return employeeInfo.ToString().TrimEnd();
        }

        // 10.	Departments with More Than 5 Employees
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var employeeInfo = new StringBuilder();
            var departments = context.Departments
                .Include(x => x.Employees)
                .Include(x => x.Manager)
                .Where(x => x.Employees.Count > 5)
                .OrderBy(x => x.Employees.Count)
                .ThenBy(x => x.Name);

            foreach (var department in departments)
            {
                employeeInfo.AppendLine($"{department.Name} - {department.Manager.FirstName}  {department.Manager.LastName}");
                
                foreach (var employee in department.Employees
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName))
                {
                    employeeInfo.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return employeeInfo.ToString().TrimEnd();
        }
    }
}
