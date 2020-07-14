namespace P03_SalesDatabase.Data
{
    using Microsoft.EntityFrameworkCore;
    using P01_HospitalDatabase.Data.Configuration;

    public class SalesContext : DbContext
    {
        public SalesContext(){}

        public SalesContext(DbContextOptions options) : base(options) { }

        // TODO appending dbset properties

        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            if (!ob.IsConfigured)
                ob.UseSqlServer(DbContextConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {

        }
    }
}