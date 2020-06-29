namespace MiniORM
{
    using System.Linq;
    using Data;
    using Data.Entities;

    class StartUp
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=.\\SQLKARAIVANOV;Database=MiniORM;Integrated Security=true";
            var dbContext = new MiniORMDbContext(connectionString);

            dbContext.Employees.Add(new Employee
            {
                FirstName = "Gosho",
                LastName =  "Inserted",
                DepartmentId= dbContext.Departments.First().Id,
                IsEmployed = true
            });

            var employee = dbContext.Employees.Last();
            employee.FirstName = "Modified";

            dbContext.SaveChanges();
        }
    }
}
