namespace P01_HospitalDatabase.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityDiagnoseConfiguration : IEntityTypeConfiguration<Diagnose>
    {
        public void Configure(EntityTypeBuilder<Diagnose> b)
        {
            b.HasKey(x => x.DiagnoseId);
            b.Property(x => x.Name)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(50);
            b.Property(x => x.Comments)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(250);
            b.HasOne(x => x.Patient)
                .WithMany(x => x.Diagnoses)
                .HasForeignKey(x => x.PatientId);
        }
    }
}