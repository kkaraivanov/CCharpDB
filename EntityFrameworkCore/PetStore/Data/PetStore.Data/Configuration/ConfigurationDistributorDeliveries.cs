namespace PetStore.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Model.Distributor;

    public class ConfigurationDistributorDeliveries : IEntityTypeConfiguration<DistributorDelivery>
    {
        public void Configure(EntityTypeBuilder<DistributorDelivery> b)
        {
            b.HasOne(x => x.Distributor)
                .WithMany(x => x.DistributorDeliverys)
                .HasForeignKey(x => x.DistributorId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(x => x.DeliveryFoods)
                .WithOne(x => x.DistributorDelivery)
                .HasForeignKey(x => x.DeliveryId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(x => x.DeliveryToys)
                .WithOne(x => x.DistributorDelivery)
                .HasForeignKey(x => x.DeliveryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}