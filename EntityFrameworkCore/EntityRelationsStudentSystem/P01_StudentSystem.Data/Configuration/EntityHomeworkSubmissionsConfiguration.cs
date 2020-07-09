namespace P01_StudentSystem.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityHomeworkSubmissionsConfiguration : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> builder)
        {
            builder
                .HasKey(p => p.HomeworkId);
            builder
                .Property(p => p.Content)
                .IsRequired(false)
                .IsUnicode(false)
                .HasMaxLength(250);
            builder
                .HasOne(p => p.Student)
                .WithMany(x => x.HomeworkSubmissions)
                .HasForeignKey(x => x.StudentId);
            builder
                .HasOne(p => p.Course)
                .WithMany(x => x.HomeworkSubmissions)
                .HasForeignKey(x => x.CourseId);
        }
    }
}