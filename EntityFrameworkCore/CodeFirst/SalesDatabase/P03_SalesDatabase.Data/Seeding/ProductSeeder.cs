namespace P03_SalesDatabase.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Managment.Contracts;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ProductSeeder : Iseeder
    {
        private readonly SalesContext dbContext;
        private readonly Random rand;
        private readonly IWriter writer;

        public ProductSeeder(SalesContext context, Random rand, IWriter writer)
        {
            dbContext = context;
            this.rand = rand;
            this.writer = writer;
        }

        public void Seed()
        {
            ICollection<Product> products = new List<Product>();
            var names = new string[]
            {
                "CPU",
                "Motherboard",
                "GPU",
                "RAM",
                "SSD",
                "HDD",
                "CD-RW",
                "Air Cooler",
                "Thermopaste"
            };

            for (int i = 0; i < 50; i++)
            {
                int nameIndex = rand.Next(0, names.Length);
                string str = names[nameIndex];
                double quantity = rand.Next(1000);
                decimal price = rand.Next(5000) * 1.133m;

                var product = new Product()
                {
                    Name = str,
                    Price = price,
                    Quantity = quantity
                };

                products.Add(product);
                writer.Write($"Product (Name:{str}");
                writer.Write($" Quantity:{quantity}");
                writer.WriteLine($" Price:{price}$) was added to the database!");
            }

            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();
        }
    }
}