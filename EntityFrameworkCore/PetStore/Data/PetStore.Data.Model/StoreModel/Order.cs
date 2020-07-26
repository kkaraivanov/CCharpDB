namespace PetStore.Data.Model.StoreModel
{
    using System;
    using System.Collections.Generic;
    using Customer;
    using FoodModel;
    using PetModel;
    using PetSore.Data.Common.Enumerators;
    using ToyModel;

    public class Order
    {
        public int Id { get; set; }

        public DateTime PurchaseDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public HashSet<Pet> Pets { get; set; } = new HashSet<Pet>();

        public HashSet<FoodOrder> FoodOrders { get; set; } = new HashSet<FoodOrder>();

        public HashSet<ToyOrders> ToyOrders { get; set; } = new HashSet<ToyOrders>();
    }
}