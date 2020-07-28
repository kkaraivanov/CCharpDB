namespace PetStore.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Model.Distributor;

    public class ConfigurationDeliveryToys : IEntityTypeConfiguration<DeliveryToy>
    {
        public void Configure(EntityTypeBuilder<DeliveryToy> b)
        {
            b.HasKey(x => new { x.DeliveryId, x.ToyId });

            b.HasOne(x => x.Toy)
                .WithMany(x => x.DeliveryToys)
                .HasForeignKey(x => x.ToyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}