namespace ProductShop.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ProductShopContext : DbContext
    {
        public ProductShopContext() { }

        public ProductShopContext(DbContextOptions options) : base(options) { }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<CategoryProduct> CategoryProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            if (!ob.IsConfigured)
                ob.UseSqlServer(DbContextConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<CategoryProduct>(entity =>
            {
                entity.HasKey(x => new { x.CategoryId, x.ProductId });

                entity.HasOne(e => e.Category)
                    .WithMany(e => e.CategoryProducts)
                    .HasForeignKey(e => e.CategoryId);

                entity.HasOne(e => e.Product)
                    .WithMany(e => e.CategoryProducts)
                    .HasForeignKey(e => e.ProductId);
            });

            mb.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired(false);

                entity.HasOne(e => e.Buyer)
                    .WithMany(u => u.ProductsBought)
                    .HasForeignKey(e => e.BuyerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Seller)
                    .WithMany(u => u.ProductsSold)
                    .HasForeignKey(e => e.SellerId);
            });
        }
    }
}