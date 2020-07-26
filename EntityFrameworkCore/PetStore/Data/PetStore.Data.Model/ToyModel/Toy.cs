namespace PetStore.Data.Model.ToyModel
{
    using System.Collections.Generic;
    using FoodModel;
    using PetModel;
    using StoreModel;

    public class Toy
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<ToyOrders> ToyOrders { get; set; } = new HashSet<ToyOrders>();
    }
}