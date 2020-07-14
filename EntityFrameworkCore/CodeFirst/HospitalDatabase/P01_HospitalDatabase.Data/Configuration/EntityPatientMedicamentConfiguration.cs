namespace P01_HospitalDatabase.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityPatientMedicamentConfiguration : IEntityTypeConfiguration<PatientMedicament>
    {
        public void Configure(EntityTypeBuilder<PatientMedicament> b)
        {
            b.HasKey(x => new {x.PatientId, x.MedicamentId});
            b.HasOne(x => x.Patient)
                .WithMany(x => x.Prescriptions)
                .HasForeignKey(x => x.PatientId);
            b.HasOne(x => x.Medicament)
                .WithMany(x => x.Prescriptions)
                .HasForeignKey(x => x.MedicamentId);
        }
    }
}