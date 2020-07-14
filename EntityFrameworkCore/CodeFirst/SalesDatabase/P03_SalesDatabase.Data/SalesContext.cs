namespace P03_SalesDatabase.Data
{
    using Microsoft.EntityFrameworkCore;
    using Configuration;
    using Models;

    public class SalesContext : DbContext
    {
        public SalesContext(){}

        public SalesContext(DbContextOptions options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            if (!ob.IsConfigured)
                ob.UseSqlServer(DbContextConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.ApplyConfiguration(new EntityCustomerConfiguration());
            mb.ApplyConfiguration(new EntityProductConfiguration());
            mb.ApplyConfiguration(new EntitySaleConfiguration());
            mb.ApplyConfiguration(new EntityStoreConfiguration());
        }
    }
}