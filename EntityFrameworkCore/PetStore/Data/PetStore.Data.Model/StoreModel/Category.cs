namespace PetStore.Data.Model.StoreModel
{
    using System.Collections.Generic;
    using FoodModel;
    using PetModel;
    using ToyModel;

    public class Category
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Pet> Pets { get; set; } = new HashSet<Pet>();

        public ICollection<Toy> Toys { get; set; } = new HashSet<Toy>();

        public ICollection<Food> Foods { get; set; } = new HashSet<Food>();
    }
}