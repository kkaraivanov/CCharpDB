namespace PetStore.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Model.ToyModel;

    public class ConfigurationToyOrders : IEntityTypeConfiguration<ToyOrder>
    {
        public void Configure(EntityTypeBuilder<ToyOrder> b)
        {
            b.HasKey(x => new { x.ToyId, x.OrderId });

            b.HasOne(x => x.Toy)
                .WithMany(x => x.ToyOrders)
                .HasForeignKey(x => x.ToyId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Order)
                .WithMany(x => x.ToyOrders)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}