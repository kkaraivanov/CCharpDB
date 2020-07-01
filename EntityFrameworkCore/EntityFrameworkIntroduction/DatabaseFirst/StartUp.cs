namespace SoftUni
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using SoftUni.Data;
    using SoftUni.Models;

    public class StartUp
    {
        private static StringBuilder returnsInfo;

        static void Main(string[] args)
        {
            returnsInfo = new StringBuilder();
            var dbContext = new SoftUniContext();

            Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(dbContext));
        }

        // 3.	Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees;

            foreach (var employee in employees)
            {
                var currentEmployee = $"{employee.FirstName} " +
                                      $"{employee.LastName} " +
                                      $"{employee.MiddleName} " +
                                      $"{employee.JobTitle} " +
                                      $"{employee.Salary:f2}";
                returnsInfo.AppendLine(currentEmployee);
            }

            return returnsInfo.ToString().TrimEnd();
        }

        // 4.	Employees with Salary Over 50 000
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.Salary > 50000)
                .OrderBy(x => x.FirstName);

            foreach (var employee in employees)
            {
                var currentEmployee = $"{employee.FirstName} - {employee.Salary:f2}"; //{firstName} - {salary}.
                returnsInfo.AppendLine(currentEmployee);
            }

            return returnsInfo.ToString().TrimEnd();
        }

        // 5.	Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
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
                returnsInfo.AppendLine(currentEmployee);
            }

            return returnsInfo.ToString().TrimEnd();
        }

        // 6.	Adding a New Address and Updating Employee
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
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
                .ForEach(x => returnsInfo.AppendLine(x));

            return returnsInfo.ToString().TrimEnd();
        }

        // 7.	Employees and Projects
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
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
                    Projects = employe.EmployeesProjects.Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        StartDate = ep.Project.StartDate
                            .ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        EndDate = ep.Project.EndDate.HasValue ? ep.Project.EndDate.Value
                                .ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"
                    })
                })
                .ToList();

            foreach (var employee in employees)
            {
                returnsInfo.AppendLine(
                    $"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}{Environment.NewLine}" +
                    $"{String.Join(Environment.NewLine, employee.Projects.Select(p => $"--{p.ProjectName} - {p.StartDate} - {p.EndDate}"))}");
            }

            return returnsInfo.ToString().TrimEnd();
        }

        // 8.	Addresses by Town
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                    .Include(x => x.Employees)
                    .Include(x => x.Town)
                    .OrderByDescending(x => x.Employees.Count)
                    .ThenBy(x => x.Town.Name)
                    .ThenBy(x => x.AddressText)
                    .Take(10)
                    .ToList();

            addresses.ForEach(x =>
                returnsInfo.AppendLine($"{x.AddressText}, {x.Town.Name} - {x.Employees.Count} employees"));

            return returnsInfo.ToString().TrimEnd();
        }

        // 9.	Employee 147
        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .First(x => x.EmployeeId == 147);

            returnsInfo.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            foreach (var project in employee.EmployeesProjects
                .OrderBy(x => x.Project.Name))
            {
                returnsInfo.AppendLine($"{project.Project.Name}");
            }

            return returnsInfo.ToString().TrimEnd();
        }

        // 10.	Departments with More Than 5 Employees
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Include(x => x.Employees)
                .Include(x => x.Manager)
                .Where(x => x.Employees.Count > 5)
                .OrderBy(x => x.Employees.Count)
                .ThenBy(x => x.Name);

            foreach (var department in departments)
            {
                returnsInfo.AppendLine($"{department.Name} - {department.Manager.FirstName}  {department.Manager.LastName}");

                foreach (var employee in department.Employees
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName))
                {
                    returnsInfo.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return returnsInfo.ToString().TrimEnd();
        }

        // 11.	Find Latest 10 Projects
        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(x => x.StartDate)
                .Take(10)
                .OrderBy(x => x.Name)
                .ToList();

            projects.ForEach(x =>
                returnsInfo.AppendLine($"{x.Name}{Environment.NewLine}" +
                                       $"{x.Description}{Environment.NewLine}" +
                                       $"{x.StartDate.ToString("M/d/yyyy h:mm:ss tt")}"));

            return returnsInfo.ToString().TrimEnd();
        }

        // 12.	Increase Salaries
        public static string IncreaseSalaries(SoftUniContext context)
        {
            context.Employees
                .Where(x =>
                    x.Department.Name == "Engineering" ||
                    x.Department.Name == "Tool Design" ||
                    x.Department.Name == "Marketing" ||
                    x.Department.Name == "Information Services")
                .ToList()
                .ForEach(employee => employee.Salary *= 1.12M);
            context.SaveChanges();

            var employees = context.Employees
                .Where(x =>
                    x.Department.Name == "Engineering" ||
                    x.Department.Name == "Tool Design" ||
                    x.Department.Name == "Marketing" ||
                    x.Department.Name == "Information Services")
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            foreach (var employee in employees)
            {
                returnsInfo.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
            }


            return returnsInfo.ToString().TrimEnd();
        }

        // 13.	Find Employees by First Name Starting With "Sa"
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.FirstName.StartsWith("Sa"))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    x.Salary
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName);
                
                foreach (var e in employees)
                {
                    string result = $"{e.FirstName} {e.LastName} - " +
                                    $"{e.JobTitle} - (${e.Salary:f2})";
                    returnsInfo.AppendLine($"{result} --> Length: {result.Length}");
                }
            //employees.ForEach(e =>
            //    returnsInfo.AppendLine($"{e.FirstName} {e.LastName} - " +
            //                           $"{e.JobTitle} - (${e.Salary:f2})"));

            return returnsInfo.ToString().TrimEnd();
        }

        // 14.	Delete Project by Id
        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectsInfo = new StringBuilder();
            var project = context.Projects.First(x => x.ProjectId == 2);
            context.EmployeesProjects
                .Where(x => x.ProjectId == project.ProjectId)
                .ToList()
                .ForEach(x =>
                    context.EmployeesProjects.Remove(x));
            context.Projects.Remove(project);
            context.SaveChanges();

            context.Projects
                .Take(10)
                .ToList()
                .ForEach(x => projectsInfo.AppendLine($"{x.Name}"));

            return projectsInfo.ToString().TrimEnd();
        }

        // 15.	Remove Town
        public static string RemoveTown(SoftUniContext context)
        {
            // deletes a town with name Seattle
            Town deleteTown = context.Towns
                .First(x => x.Name == "Seattle");
            IQueryable<Address> deleteAddress = context.Addresses
                .Where(x => x.TownId == deleteTown.TownId);
            var countAddress = deleteAddress.Count();
            IQueryable<Employee> deleteEmployee = context.Employees
                .Where(x => deleteAddress
                    .Any(a => a.AddressId == x.AddressId));
            foreach (Employee employee in deleteEmployee)
            {
                employee.AddressId = null;
            }

            foreach (Address address in deleteAddress)
            {
                context.Addresses.Remove(address);
            }

            context.Towns.Remove(deleteTown);
            context.SaveChanges();

            return $"{countAddress} addresses in {deleteTown.Name} were deleted";
            //returnsInfo.AppendLine($"{countAddress} " +
            //                       $"{(countAddress == 1 ? "address" : "addresses")} in {town} " +
            //                       $"{(countAddress == 1 ? "was" : "were")} deleted");
        }
    }
}
