namespace PetStore.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Model.Distributor;

    public class ConfigurationDeliveryFoods : IEntityTypeConfiguration<DeliveryFood>
    {
        public void Configure(EntityTypeBuilder<DeliveryFood> b)
        {
            b.HasKey(x => new {x.DeliveryId, x.FoodId});

            b.HasOne(x => x.Food)
                .WithMany(x => x.DeliveryFoods)
                .HasForeignKey(x => x.FoodId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}