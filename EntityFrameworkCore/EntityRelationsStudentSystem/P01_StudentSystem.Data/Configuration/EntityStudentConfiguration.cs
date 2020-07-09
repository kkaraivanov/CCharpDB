namespace P01_StudentSystem.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityStudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder
                .HasKey(p => p.StudentId);
            builder
                .Property(p => p.Name)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(100);
            builder
                .Property(p => p.PhoneNumber)
                .IsRequired(false)
                .IsUnicode(false)
                .HasMaxLength(10);
            builder
                .Property(p => p.RegisteredOn)
                .IsRequired(true);
        }
    }
}