﻿namespace PetStore.Data.Model.StoreModel
{
    using System.Collections.Generic;
    using FoodModel;
    using ToyModel;

    public class Brand
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Toy> Toys { get; set; } = new HashSet<Toy>();

        public ICollection<Food> Foods { get; set; } = new HashSet<Food>();
    }
}