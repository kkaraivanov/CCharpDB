namespace P03_SalesDatabase.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntitySaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> b)
        {
            b.HasKey(x => x.SaleId);
            b.Property(x => x.Date)
                .HasDefaultValueSql("GETDATE()");
            b.HasOne(x => x.Product)
                .WithMany(x => x.Sales)
                .HasForeignKey(x => x.SaleId);
            b.HasOne(x => x.Customer)
                .WithMany(x => x.Sales)
                .HasForeignKey(x => x.CustomerId);
            b.HasOne(x => x.Store)
                .WithMany(x => x.Sales)
                .HasForeignKey(x => x.StoreId);
        }
    }
}