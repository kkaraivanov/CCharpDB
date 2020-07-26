namespace PetStore.Data.Model.StoreModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Customer;
    using FoodModel;
    using PetModel;
    using PetSore.Data.Common.Enumerators;
    using ToyModel;

    public class Order
    {
        [Key]
        public int Id { get; set; }

        public DateTime PurchaseDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Pet> Pets { get; set; } = new HashSet<Pet>();

        public ICollection<FoodOrder> FoodOrders { get; set; } = new HashSet<FoodOrder>();

        public ICollection<ToyOrders> ToyOrders { get; set; } = new HashSet<ToyOrders>();
    }
}