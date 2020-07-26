namespace PetStore.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Model.FoodModel;

    public class ConfigurationFoodOrders : IEntityTypeConfiguration<FoodOrder>
    {
        public void Configure(EntityTypeBuilder<FoodOrder> b)
        {
            b.HasKey(x => new {x.FoodId, x.OrderId});

            b.HasOne(x => x.Food)
                .WithMany(x => x.FoodOrders)
                .HasForeignKey(x => x.FoodId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Order)
                .WithMany(x => x.FoodOrders)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}