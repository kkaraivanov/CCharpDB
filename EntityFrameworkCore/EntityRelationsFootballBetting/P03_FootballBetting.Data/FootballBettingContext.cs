﻿namespace P03_FootballBetting.Data
{
    using Configuration;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext() { }

        public FootballBettingContext(DbContextOptions options) : base(options) { }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            if (!ob.IsConfigured)
                ob.UseSqlServer(DbConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.ApplyConfiguration(new EntityTeamConfiguration());
            mb.ApplyConfiguration(new EntityColorConfiguration());
            mb.ApplyConfiguration(new EntityTownConfiguration());
            mb.ApplyConfiguration(new EntityCountryConfiguration());
            mb.ApplyConfiguration(new EntityPlayerConfiguration());
            mb.ApplyConfiguration(new EntityPlayerStatisticConfiguration());
            mb.ApplyConfiguration(new EntityPositionConfiguration());
            mb.ApplyConfiguration(new EntityGameConfiguration());
            mb.ApplyConfiguration(new EntityBetConfiguration());
            mb.ApplyConfiguration(new EntityUserConfiguration());
        }
    }
}
