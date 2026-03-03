using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<FacultyCourse> FacultyCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Course -> Department (required)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Faculty -> Department (required)
            modelBuilder.Entity<Faculty>()
                .HasOne(f => f.Department)
                .WithMany(d => d.Faculties)
                .HasForeignKey(f => f.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // FacultyCourse mapping: unique constraint to prevent duplicates
            modelBuilder.Entity<FacultyCourse>()
                .HasIndex(fc => new { fc.FacultyId, fc.CourseId })
                .IsUnique();

            // FacultyCourse relationships
            modelBuilder.Entity<FacultyCourse>()
                .HasOne(fc => fc.Faculty)
                .WithMany()
                .HasForeignKey(fc => fc.FacultyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FacultyCourse>()
                .HasOne(fc => fc.Course)
                .WithMany()
                .HasForeignKey(fc => fc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}