using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Entities;

namespace UniAttend.Infrastructure.Data.Configurations
{
    internal class CourseSessionConfiguration : IEntityTypeConfiguration<CourseSession>
    {
        public void Configure(EntityTypeBuilder<CourseSession> builder)
        {
            builder.ToTable("CourseSessions");
            
            builder.HasKey(x => x.Id);
            
            // Use backing fields for immutable properties
            builder.Property("CourseId").IsRequired();
            builder.Property("GroupId").IsRequired();
            builder.Property("ClassroomId").IsRequired();
            builder.Property("Date").IsRequired();
            builder.Property("StartTime").IsRequired();
            builder.Property("EndTime").IsRequired();
            builder.Property("Status").IsRequired().HasMaxLength(50);

            // Configure relationships using string names to avoid navigation property exposure
            builder.HasOne(typeof(Course))
                .WithMany()
                .HasForeignKey("CourseId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(typeof(StudyGroup))
                .WithMany()
                .HasForeignKey("GroupId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(typeof(Classroom))
                .WithMany()
                .HasForeignKey("ClassroomId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex("CourseId", "Date");
        }
    }
}