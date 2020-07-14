namespace P03_SalesDatabase.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> b)
        {
            b.HasKey(x => x.ProductId);
            b.Property(x => x.Name)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(50);
            b.Property(x => x.Description)
                .IsUnicode(true)
                .HasMaxLength(250)
                .HasDefaultValue("No description");
        }
    }
}