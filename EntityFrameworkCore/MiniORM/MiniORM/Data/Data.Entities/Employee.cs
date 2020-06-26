namespace MiniORM.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string JobTitle { get; set; }

        public bool IsEmployed { get; set; }

        [ForeignKey(nameof(Departments))]
        public Department DepartmentId { get; set; }

        [ForeignKey(nameof(Employees))]
        public Employee ManagerId { get; set; }

        [Required]
        [Column(TypeName = "decimal(16, 2)")]
        public decimal Salary { get; set; }

        [ForeignKey(nameof(Address))]
        public int AddressId { get; set; }

        public ICollection<Employee> Employees { get; }

        public ICollection<Department> Departments { get; }

        public ICollection<EmployeeProject> EmployeeProjects { get; }
    }
}
