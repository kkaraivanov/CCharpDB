namespace PetStore
{
    using System;
    using System.Collections.Generic;
    using Data;
    using PetSore.Services.Busines;
    using Services.Model.Toy;

    public class StartUp
    {
        static void Main()
        {
            var db = new PetStoreDbContext();
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();

            var category = new CategoryService(db);
            var importCategory = new List<string>
            {
                "Dogs", "Cats", "Crocodile", "Banana", "Rubber Bear", "Toys"
            };
            importCategory.ForEach(x => category.Create(x));

            var toys = new ToyService(db);
            var importToys = new List<Toys>
            {
                new Toys{Brand = "Rubber", Category = "Toys", Description = "rubber toy for pets", Name = "RubberBear", Price = 10.50m},
                new Toys{Brand = "Green", Category = "Banana", Description = "Green bananas", Name = "GreenBananas", Price = 2.00m},
                new Toys{Brand = "Yelow", Category = "Banana", Description = "Yelow bananas", Name = "YelowBananas", Price = 6.90m},
                new Toys{Brand = "Rubber", Category = "Crocodile", Description = "Rubber Crocodile Aligator", Name = "Aligator", Price = 60.00m},
                new Toys{Brand = "Rubber", Category = "Crocodile", Description = "Rubber Crocodile Ordinare", Name = "OrdinareCrocodile", Price = 30.00m}
            };
            foreach (var toy in importToys)
            {
                toys.Create(toy.Name, toy.Description, toy.Price, toy.Brand, toy.Category);
            }

            var distributor = new DistributorService(db);
            distributor.Create("Anmimal Food LSCo", "FR7630006000011234567890189");
            distributor.Create("Anmimal Toy LSCo", "FR7630006000011234567890187");
            distributor.Create("Zoo Toy LSCo", "FR7630006000011234567890188");
            distributor.Create("Pet Food LSCo", "FR7630006000011234567890186");

            var customer = new CustomerService(db);
            customer.Create("Petar Petrov", "petrov@abv.bg", 0888888888);
            customer.Create("Ivan Ivanov", "ivanov@abv.bg", 0888888888);
            customer.Create("Stoyan Stoyanov", "stoyanov@abv.bg", 0888888888);
            customer.Create("Ivan Petrov", "ipetrovv@abv.bg", 0888888888);
            customer.Create("Petar Stoyanov", "pstoyanov@abv.bg", 0888888888);
            customer.Create("Stoyan Petrov", "spetrovv@abv.bg", 0888888888);

            var purchase = new PurchaseService(db);
            var import = new List<Toys>
            {
                new Toys{Brand = "Rubber", Category = "Toys", Description = "rubber toy for pets", Name = "RubberBear", Price = 10.50m, Quantity = 20},
                new Toys{Brand = "Green", Category = "Banana", Description = "Green bananas", Name = "GreenBananas", Price = 2.00m, Quantity = 40},
                new Toys{Brand = "Yelow", Category = "Banana", Description = "Yelow bananas", Name = "YelowBananas", Price = 6.90m, Quantity = 5},
                new Toys{Brand = "Rubber", Category = "Crocodile", Description = "Rubber Crocodile Aligator", Name = "Aligator", Price = 60.00m, Quantity = 25},
                new Toys{Brand = "Rubber", Category = "Crocodile", Description = "Rubber Crocodile Ordinare", Name = "OrdinareCrocodile", Price = 30.00m}
            };
            purchase.PurchaseToys("Ivan Petrov", "Anmimal Toy LSCo", import.ToArray());

            var saleToy = new SalesService(db);
            var result = saleToy.SaleToy("YelowBananas", "Stoyan Petrov", 2);
            Console.WriteLine(result);
            var statistic = saleToy.Statistic();
            Console.WriteLine(statistic);
        }
    }
}
