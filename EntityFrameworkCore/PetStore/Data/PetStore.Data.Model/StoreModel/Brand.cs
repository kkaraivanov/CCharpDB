namespace PetStore.Data.Model.StoreModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using FoodModel;
    using ToyModel;
    using static DataValidationAttribute;

    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLenght)]
        public string Name { get; set; }

        public ICollection<Toy> Toys { get; set; } = new HashSet<Toy>();

        public ICollection<Food> Foods { get; set; } = new HashSet<Food>();
    }
}