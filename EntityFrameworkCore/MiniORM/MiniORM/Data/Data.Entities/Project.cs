namespace MiniORM.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        //[Column(TypeName = "ntext")]
        //public string Description { get; set; }

        //public DateTime StartDate { get; set; }

        //public DateTime EndDate { get; set; }

        public ICollection<EmployeeProject> EmployeeProjects { get; }
    }
}
