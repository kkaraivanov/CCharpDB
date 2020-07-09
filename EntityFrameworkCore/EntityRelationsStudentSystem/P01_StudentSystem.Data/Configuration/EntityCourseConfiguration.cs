namespace P01_StudentSystem.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityCourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder
                .HasKey(p => p.CourseId);
            builder
                .Property(p => p.Name)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(80);
            builder
                .Property(p => p.Description)
                .IsRequired(false)
                .IsUnicode(true)
                .HasMaxLength(250);
        }
    }
}