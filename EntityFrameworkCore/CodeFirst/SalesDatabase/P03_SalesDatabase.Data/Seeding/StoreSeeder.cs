namespace P03_SalesDatabase.Data.Seeding
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Managment.Contracts;
    using Models;

    public class StoreSeeder : Iseeder
    {
        private readonly SalesContext dbContext;
        private readonly IWriter writer;

        public StoreSeeder(SalesContext context, IWriter writer)
        {
            dbContext = context;
            this.writer = writer;
        }

        public void Seed()
        {
            ICollection<Store> storess = new List<Store>()
            {
                new Store(){Name = "Vali Computers"},
                new Store(){Name = "Jumbo Jet"},
                new Store(){Name = "Storm Computers"},
                new Store(){Name = "Mirosoft"},
                new Store(){Name = "Aple"}
            };

            dbContext.Stores.AddRange(storess);
            dbContext.SaveChanges();
            writer.WriteLine($"{storess.Count} stores were added to the database!");
        }
    }
}