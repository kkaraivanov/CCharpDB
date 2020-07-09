namespace P03_FootballBetting.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityBetConfiguration : IEntityTypeConfiguration<Bet>
    {
        public void Configure(EntityTypeBuilder<Bet> builder)
        {
            builder
                .HasKey(p => p.BetId);
            builder
                .HasOne(p => p.User)
                .WithMany(x => x.Bets)
                .HasForeignKey(x => x.UserId);
            builder
                .HasOne(p => p.Game)
                .WithMany(x => x.Bets)
                .HasForeignKey(x => x.GameId);

        }
    }
}