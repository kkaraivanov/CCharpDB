namespace P01_StudentSystem.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EntityResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder
                .HasKey(p => p.ResourceId);
            builder
                .Property(p => p.Name)
                .IsRequired(true)
                .IsUnicode(true)
                .HasMaxLength(50);
            builder
                .HasOne(p => p.Course)
                .WithMany(x => x.Resources)
                .HasForeignKey(x => x.CourseId);
        }
    }
}