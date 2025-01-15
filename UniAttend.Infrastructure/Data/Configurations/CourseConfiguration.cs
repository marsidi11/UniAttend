using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities;

namespace UniAttend.Infrastructure.Data.Configurations
{
    internal class CourseConfiguration : EntityConfiguration<Course>
    {
        public override void Configure(EntityTypeBuilder<Course> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("Courses");
                        
            // Use backing fields for immutable properties
            builder.Property("Name")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property("Description")
                .IsRequired()
                .HasMaxLength(500);

            builder.Property("StartTime")
                .IsRequired();

            builder.Property("EndTime")
                .IsRequired();

            builder.Property("Location")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property("IsActive")
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property("ProfessorId")
                .IsRequired();

            builder.Property("StudyGroupId")
                .IsRequired();

            // Configure relationships
            builder.HasOne(typeof(Professor))
                .WithMany()
                .HasForeignKey("ProfessorId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(typeof(StudyGroup))
                .WithMany()
                .HasForeignKey("StudyGroupId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.AttendanceRecords)
                .WithOne(x => x.Course)
                .HasForeignKey("CourseId")
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            builder.HasIndex("ProfessorId", "StudyGroupId");
            builder.HasIndex("StartTime", "EndTime");
        }
    }
}