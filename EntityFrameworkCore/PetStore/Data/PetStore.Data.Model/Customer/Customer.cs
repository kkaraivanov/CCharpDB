namespace PetStore.Data.Model.Customer
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using StoreModel;
    using static DataValidationAttribute;

    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLenght)]
        public string FirstName { get; set; }

        public string LasttName { get; set; }

        [Required]
        [MaxLength(EmailMaxLenght)]
        public string Email { get; set; }


        public int PhoneNumber { get; set; }

        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}