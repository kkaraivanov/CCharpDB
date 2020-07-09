namespace P03_FootballBetting.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityCountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder
                .HasKey(p => p.CountryId);
            builder
                .Property(p => p.Name)
                .IsRequired(true)
                .IsUnicode(false)
                .HasMaxLength(50);
        }
    }
}