namespace P03_FootballBetting.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext() { }

        public FootballBettingContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            if(!ob.IsConfigured)
                ob.UseSqlServer(DbConfiguration.ConnectionString);
        }


    }
}
