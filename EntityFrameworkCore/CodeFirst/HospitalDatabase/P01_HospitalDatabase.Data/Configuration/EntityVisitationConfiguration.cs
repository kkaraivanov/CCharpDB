namespace P01_HospitalDatabase.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityVisitationConfiguration : IEntityTypeConfiguration<Visitation>
    {
        public void Configure(EntityTypeBuilder<Visitation> b)
        {
            b.HasKey(x => x.VisitationId);
            b.Property(x => x.Comments)
                .IsRequired(false)
                .IsUnicode(true)
                .HasMaxLength(250);
            b.HasOne(x => x.Patient)
                .WithMany(x => x.Visitations)
                .HasForeignKey(x => x.PatientId);
            b.HasOne(x => x.Doctor)
                .WithMany(x => x.Visitations)
                .HasForeignKey(x => x.DoctorId);
        }
    }
}