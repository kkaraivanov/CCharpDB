namespace PetStore.Data
{
    using Configuration;
    using Microsoft.EntityFrameworkCore;
    using Model.Customer;
    using Model.Distributor;
    using Model.FoodModel;
    using Model.PetModel;
    using Model.StoreModel;
    using Model.ToyModel;

    public class PetStoreDbContext : DbContext
    {
        public PetStoreDbContext()
        {
        }

        public PetStoreDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Food> Foods { get; set; }

        public DbSet<Bread> Breads { get; set; }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Toy> Toys { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ToyOrders> ToyOrderses { get; set; }

        public DbSet<Distributor> Distributors { get; set; }

        public DbSet<DistributorDelivery> DistributorDeliveries { get; set; }

        public DbSet<DeliveryToy> DeliveryToies { get; set; }

        public DbSet<DeliveryFood> DeliveryFoods { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbContextConfiguration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.ApplyConfiguration(new ConfigurationBrands());
            mb.ApplyConfiguration(new ConfigurationPet());
            mb.ApplyConfiguration(new ConfigurationCategories());
            mb.ApplyConfiguration(new ConfigurationOrders());
            mb.ApplyConfiguration(new ConfigurationToyOrders());
            mb.ApplyConfiguration(new ConfigurationFoodOrders());
            mb.ApplyConfiguration(new ConfigurationCusotmers());
            mb.ApplyConfiguration(new ConfigurationDeliveryFoods());
            mb.ApplyConfiguration(new ConfigurationDeliveryToys());
            mb.ApplyConfiguration(new ConfigurationDistributorDeliveries());
        }
    }
}