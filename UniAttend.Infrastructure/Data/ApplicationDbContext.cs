using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Identity;
using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Entities.Audit;
using UniAttend.Infrastructure.Data.Configurations;

namespace UniAttend.Infrastructure.Data
{
    /// <summary>
    /// Represents the main database context for the application,
    /// handling entity configurations and database operations.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudyGroup> StudyGroups { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseSession> CourseSession { get; set; }
        public DbSet<OtpCode> OtpCodes { get; set; }
        public DbSet<GroupStudent> GroupStudents { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
        .Property(e => e.Role)
        .HasConversion<string>()
        .HasMaxLength(50);
        
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}