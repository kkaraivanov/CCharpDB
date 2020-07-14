namespace P01_HospitalDatabase.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityMedicamentConfiguration : IEntityTypeConfiguration<Medicament>
    {
        public void Configure(EntityTypeBuilder<Medicament> b)
        {
            b.HasKey(x => x.MedicamentId);
            b.Property(x => x.Name)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(50);
        }
    }
}