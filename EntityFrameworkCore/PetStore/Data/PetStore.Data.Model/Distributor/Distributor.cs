namespace PetStore.Data.Model.Distributor
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataValidationAttribute;

    public class Distributor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLenght)]
        public string Name { get; set; }

        [Required]
        [MaxLength(bankAccountMaxLenght)]
        [MinLength(bankAccountMinLenght)]
        public string BankAccount { get; set; }

        public ICollection<DistributorDelivery> DistributorDeliveries { get; set; } = new HashSet<DistributorDelivery>();
    }
}