namespace MiniORM.Data
{
    using Core;
    using Entities;

    public class MiniORMDbContext : DbContext
    {
        public MiniORMDbContext(string connectionString)
            : base(connectionString) { }

        public DbSet<Employee> Employees { get; }

        public DbSet<Department> Departments { get; }

        //public DbSet<Address> Addresses { get; }

        //public DbSet<Town> Towns { get; }

        public DbSet<Project> Projects { get; }

        public DbSet<EmployeeProject> EmployeesProjects { get; }
    }
}
