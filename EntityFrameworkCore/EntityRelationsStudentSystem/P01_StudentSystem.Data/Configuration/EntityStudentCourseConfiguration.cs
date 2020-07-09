namespace P01_StudentSystem.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityStudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {
            builder
                .HasKey(p => new {p.StudentId, p.CourseId});
            builder
                .HasOne(p => p.Student)
                .WithMany(x => x.CourseEnrollments)
                .HasForeignKey(x => x.StudentId);
            builder
                .HasOne(p => p.Course)
                .WithMany(x => x.StudentsEnrolled)
                .HasForeignKey(x => x.CourseId);
        }
    }
}