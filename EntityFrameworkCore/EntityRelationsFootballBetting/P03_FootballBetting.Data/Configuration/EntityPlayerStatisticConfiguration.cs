namespace P03_FootballBetting.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityPlayerStatisticConfiguration : IEntityTypeConfiguration<PlayerStatistic>
    {
        public void Configure(EntityTypeBuilder<PlayerStatistic> builder)
        {
            builder
                .HasKey(p => new {p.GameId, p.PlayerId});
            builder
                .HasOne(p => p.Game)
                .WithMany(x => x.PlayerStatistics)
                .HasForeignKey(x => x.GameId);
            builder
                .HasOne(p => p.Player)
                .WithMany(x => x.PlayerStatistics)
                .HasForeignKey(x => x.PlayerId);
        }
    }
}