namespace P01_HospitalDatabase.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityPatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> b)
        {
            b.HasKey(x => x.PatientId);
            b.Property(x => x.FirstName)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(50);
            b.Property(x => x.LastName)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(50);
            b.Property(x => x.Address)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(250);
            b.Property(x => x.Email)
                .IsRequired(true)
                .IsUnicode(false)
                .HasMaxLength(80);
            b.Property(x => x.HasInsurance)
                .HasDefaultValue("false");
        }
    }
}