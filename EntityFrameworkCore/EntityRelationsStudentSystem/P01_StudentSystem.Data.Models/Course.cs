namespace P01_StudentSystem.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Course
    {
        public Course()
        {
            StudentsEnrolled = new HashSet<StudentCourse>();
            HomeworkSubmissions = new HashSet<Homework>();
            Resources = new HashSet<Resource>();
        }

        public int CourseId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<StudentCourse> StudentsEnrolled { get; set; }

        public virtual ICollection<Homework> HomeworkSubmissions { get; set; }

        public ICollection<Resource> Resources { get; set; }
    }
}