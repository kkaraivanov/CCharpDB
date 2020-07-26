namespace PetStore.Data.Model.FoodModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using StoreModel;
    using static DataValidationAttribute;

    public class Food
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLenght)]
        public string Name { get; set; }

        public double Weight { get; set; }

        public decimal Price { get; set; }

        [Required]
        public DateTime EsprirationDate { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<FoodOrder> FoodOrders { get; set; } = new HashSet<FoodOrder>();
    }
}