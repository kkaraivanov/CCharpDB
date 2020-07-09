namespace P03_FootballBetting.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityPlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder
                .HasKey(p => p.PlayerId);
            builder
                .Property(p => p.Name)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(100);
            builder
                .HasOne(p => p.Team)
                .WithMany(x => x.Players)
                .HasForeignKey(x => x.TeamId);
            builder
                .HasOne(p => p.Position)
                .WithMany(x => x.Players)
                .HasForeignKey(x => x.PositionId);
        }
    }
}