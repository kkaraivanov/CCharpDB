namespace P01_StudentSystem.Data
{
    using Configuration;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext() { }

        public StudentSystemContext(DbContextOptions options) : base(options) { }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            if (!ob.IsConfigured)
                ob.UseSqlServer(DbConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.ApplyConfiguration(new EntityCourseConfiguration());
            mb.ApplyConfiguration(new EntityHomeworkSubmissionsConfiguration());
            mb.ApplyConfiguration(new EntityResourceConfiguration());
            mb.ApplyConfiguration(new EntityStudentConfiguration());
            mb.ApplyConfiguration(new EntityStudentCourseConfiguration());
        }
    }
}