namespace PetStore.Data.Model.PetModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataValidationAttribute;

    public class Bread
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLenght)]
        public string Name { get; set; }

        public ICollection<Pet> Pets { get; set; } = new HashSet<Pet>();
    }
}