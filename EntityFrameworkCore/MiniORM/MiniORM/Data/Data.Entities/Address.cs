namespace MiniORM.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [Required]
        [MaxLength(100)]
        public string AddressText { get; set; }

        [ForeignKey(nameof(Entities.Town))]
        public int TownId { get; set; }

        public Town Town { get; set; }

        public ICollection<Employee> Employees { get; }
    }
}
