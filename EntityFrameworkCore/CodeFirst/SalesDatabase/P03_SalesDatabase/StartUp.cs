namespace P03_SalesDatabase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Data.Managment;
    using Data.Models;
    using Data.Seeding;
    using Data.Seeding.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        static void Main()
        {
            var db = new SalesContext();
            db.Database.EnsureCreated();
            //var rand = new Random();
            //var writer = new ConsoleWriter();

            //ICollection<Iseeder> seeders = new List<Iseeder>();
            //seeders.Add(new ProductSeeder(db, rand, writer));
            //seeders.Add(new StoreSeeder(db, writer));

            //foreach (var seeder in seeders)
            //{
            //    seeder.Seed();
            //}

            //var sale = new Sale()
            //{
            //    CustomerId = 1,
            //    ProductId = 10,
            //    StoreId = 1
            //};
            //db.Sales.Add(sale);
            //db.SaveChanges();

            //Sale[] sales = db.Sales.ToArray();
            //foreach (var sale1 in sales)
            //{
            //    Console.WriteLine($"{sale1.SaleId} {sale1.Date}");
            //}
        }
    }
}
