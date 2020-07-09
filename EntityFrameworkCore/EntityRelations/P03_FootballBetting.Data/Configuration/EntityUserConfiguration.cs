namespace P03_FootballBetting.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityUserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(p => p.UserId);
            builder
                .Property(p => p.Username)
                .IsRequired(true)
                .IsUnicode(false)
                .HasMaxLength(50);
            builder
                .Property(p => p.Password)
                .IsRequired(true)
                .IsUnicode(false)
                .HasMaxLength(250);
            builder
                .Property(p => p.Email)
                .IsRequired(true)
                .IsUnicode(false)
                .HasMaxLength(100);
            builder
                .Property(p => p.Name)
                .IsRequired(false)
                .IsUnicode(true)
                .HasMaxLength(100);
        }
    }
}