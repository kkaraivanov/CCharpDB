namespace CarDealer.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class CarDealerContext : DbContext
    {
        public CarDealerContext(DbContextOptions options)
            : base(options)
        {
        }

        public CarDealerContext()
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<PartCar> PartCars { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbContextConfiguration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PartCar>(e =>
            {
                e.HasKey(k => new { k.CarId, k.PartId });

                e.HasOne(x => x.Part)
                    .WithMany(x => x.PartCars)
                    .HasForeignKey(x => x.PartId);

                e.HasOne(x => x.Car)
                    .WithMany(x => x.PartCars)
                    .HasForeignKey(x => x.CarId);
            });

            modelBuilder.Entity<Sale>(e =>
            {
                e.HasKey(k => k.Id);

                e.HasOne(x => x.Customer)
                    .WithMany(x => x.Sales)
                    .HasForeignKey(x => x.CustomerId);

                e.HasOne(x => x.Car)
                    .WithMany(x => x.Sales)
                    .HasForeignKey(x => x.CarId);
            });
        }
    }
}
