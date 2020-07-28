namespace PetStore.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Model.Distributor;

    public class ConfigurationDistributor : IEntityTypeConfiguration<Distributor>
    {
        public void Configure(EntityTypeBuilder<Distributor> b)
        {
            b.HasMany(x => x.DistributorDeliveries)
                .WithOne(x => x.Distributor)
                .HasForeignKey(x => x.DistributorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}