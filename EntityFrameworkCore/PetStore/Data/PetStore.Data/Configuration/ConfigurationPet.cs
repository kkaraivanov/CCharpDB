namespace PetStore.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Model.PetModel;

    public class ConfigurationPet : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> b)
        {
            b.HasKey(x => x.Id);

            b.HasOne(x => x.Bread)
                .WithMany(x => x.Pets)
                .HasForeignKey(x => x.BreadId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Image)
                .WithMany(x => x.Pets)
                .HasForeignKey(x => x.ImageId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Order)
                .WithMany(x => x.Pets)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}