namespace P03_FootballBetting.Data.Configuration
{
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityTeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder
                .HasKey(p => p.TeamId);
            builder
                .Property(p => p.Name)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(50);
            builder
                .Property(p => p.LogoUrl)
                .IsRequired(true)
                .IsUnicode(false);
            builder
                .Property(p => p.Initials)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(3);
            builder
                .HasOne(p => p.PrimaryKitColor)
                .WithMany(x => x.PrimaryKitTeams)
                .HasForeignKey(x => x.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(p => p.SecondaryKitColor)
                .WithMany(x => x.SecondaryKitTeams)
                .HasForeignKey(x => x.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(p => p.Town)
                .WithMany(x => x.Teams)
                .HasForeignKey(x => x.TownId);
        }
    }
}