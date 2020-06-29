namespace MiniORM.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        //[Required]
        //[ForeignKey(nameof(Employee))]
        //public int ManagerId { get; set; }

        public ICollection<Employee> Employees { get; }
    }
}
