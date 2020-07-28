namespace PetSore.Services.Busines
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Common.Enumerators;
    using PetStore.Data;
    using PetStore.Data.Model.Distributor;
    using PetStore.Data.Model.StoreModel;
    using PetStore.Services.Model.Toy;

    public class PurchaseService
    {
        private readonly PetStoreDbContext db;

        public PurchaseService(PetStoreDbContext context) => db = context;

        public void PurchaseToys(string customerName, string distributorName, Toys[] toys)
        {
            var firstName = customerName.Split(" ")[0];
            var lastName = customerName.Split(" ")[1];
            var currentTime = DateTime.Now;
            var order = new Order
            {
                PurchaseDate = currentTime,
                OrderStatus = OrderStatus.Paid,
                CustomerId = db.Customers
                    .Where(x => x.FirstName == firstName && x.LasttName == lastName)
                    .Select(x => x.Id)
                    .First()
            };
            db.Orders.Add(order);
            db.SaveChanges();

            var sum = toys.Sum(x => x.Price * x.Quantity);
            var distributorId = db.Distributors
                .Where(x => x.Name.Equals(distributorName))
                .Select(x => x.Id)
                .First();
            var orderId = db.Orders.Select(x => x.Id).Max();
            var delivery = new DistributorDelivery
            {
                DeliveryDate = currentTime,
                Cost = sum,
                DistributorId = distributorId,
                OrderId = orderId
            };
            db.DistributorDeliveries.Add(delivery);
            db.SaveChanges();

            var deliveryToys = new List<DeliveryToy>();
            foreach (var toy in toys)
            {
                var currentToy = new ToyService(db);
                var ToyId = db.Toys
                    .Where(x => x.Name == toy.Name)
                    .Select(x => x.Id).First();

                var dFood = new DeliveryToy
                {
                    DeliveryId = delivery.Id,
                    ToyId = ToyId
                };
                deliveryToys.Add(dFood);
                currentToy.Create(toy.Name, toy.Description, toy.Price, toy.Brand, toy.Category, toy.Quantity);
            }
            db.DeliveryToies.AddRange(deliveryToys);
            db.SaveChanges();
        }
    }
}