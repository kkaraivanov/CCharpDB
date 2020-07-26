namespace PetStore.Data.Model.FoodModel
{
    using System;
    using System.Collections.Generic;
    using StoreModel;

    public class Food
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Weight { get; set; }

        public decimal Price { get; set; }

        public DateTime EprirationDate { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<FoodOrder> FoodOrders { get; set; } = new HashSet<FoodOrder>();
    }
}