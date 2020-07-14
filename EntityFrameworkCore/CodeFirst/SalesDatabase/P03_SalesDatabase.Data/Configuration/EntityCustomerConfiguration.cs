namespace P03_SalesDatabase.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityCustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> b)
        {
            b.HasKey(x => x.CustomerId);
            b.Property(x => x.Name)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(100);
            b.Property(x => x.Email)
                .IsRequired(true)
                .IsUnicode(false)
                .HasMaxLength(100);
            b.Property(x => x.CreditCardNumber)
                .IsRequired(true)
                .IsUnicode(false)
                .HasMaxLength(20);
        }
    }
}