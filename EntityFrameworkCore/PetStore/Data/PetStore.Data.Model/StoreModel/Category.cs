namespace PetStore.Data.Model.StoreModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using FoodModel;
    using PetModel;
    using ToyModel;
    using static DataValidationAttribute;

    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLenght)]
        public string Name { get; set; }

        [MaxLength(NameMaxLenght)]
        public string Description { get; set; }

        public ICollection<Pet> Pets { get; set; } = new HashSet<Pet>();

        public ICollection<Toy> Toys { get; set; } = new HashSet<Toy>();

        public ICollection<Food> Foods { get; set; } = new HashSet<Food>();
    }
}