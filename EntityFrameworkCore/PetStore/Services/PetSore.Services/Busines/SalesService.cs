namespace PetSore.Services.Busines
{
    using System;
    using System.Linq;
    using Data.Common.Enumerators;
    using PetStore.Data;
    using PetStore.Data.Model.StoreModel;
    using PetStore.Data.Model.ToyModel;

    public class SalesService
    {
        private readonly PetStoreDbContext db;

        public SalesService(PetStoreDbContext context) => db = context;

        public string SaleToy(string toyName, string customerName, int quantity)
        {
            var toy = db.Toys.First(x => x.Name.Equals(toyName));
            var customer = db.Customers.First(x => 
                x.FirstName.Equals(customerName.Split()[0]) &&
                x.LasttName.Equals(customerName.Split()[1]));
            if (toy.Quantity < quantity)
                return $"You have {toy.Quantity} number available";

            toy.Quantity -= quantity;
            db.Toys.Update(toy);
            
            var order = new Order
            {
                PurchaseDate = DateTime.Now,
                OrderStatus = OrderStatus.Pending,
                CustomerId = customer.Id,
            };
            
            db.Orders.Add(order);
            db.SaveChanges();
            
            var orderId = db.Orders.Select(x => x.Id).Max();
            var toyOrders = new ToyOrders
            {
                ToyId = toy.Id,
                OrderId = orderId
            };
            db.ToyOrderses.Add(toyOrders);
            db.SaveChanges();

            return quantity == 1 ? 
                $"You sale {quantity} toy for ${toy.Price * quantity}" :
                $"You sale {quantity} toys for ${toy.Price * quantity}";
        }

        public string Statistic()
        {
            var toys = db.Toys.Select(x => new
            {
                Price =  x.Price,
                Quantity = x.Quantity
            }).ToList();
            var totalToysPrice = toys
                .Where(x => x.Quantity != 0)
                .Sum(x => x.Price * x.Quantity);
            var cost = db.DistributorDeliveries.Sum(x => x.Cost);
            var incoming = cost - totalToysPrice;

            return $"Costs of purchased products ${cost}{Environment.NewLine}" +
                   $"Revenue from services sold ${incoming}";
        }
    }
}