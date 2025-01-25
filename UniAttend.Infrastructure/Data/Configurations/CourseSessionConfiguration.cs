using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities.Attendance;

namespace UniAttend.Infrastructure.Data.Configurations
{
    internal class CourseSessionConfiguration : EntityConfiguration<CourseSession>
    {
        public override void Configure(EntityTypeBuilder<CourseSession> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("CourseSessions");
                        
            // Properties
            builder.Property(x => x.StudyGroupId).IsRequired();
            builder.Property(x => x.ClassroomId).IsRequired();
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.StartTime).IsRequired();
            builder.Property(x => x.EndTime).IsRequired();
            builder.Property(x => x.Status).IsRequired().HasMaxLength(50);

            // Relationships
            builder.HasOne(x => x.StudyGroup)
                .WithMany()
                .HasForeignKey(x => x.StudyGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Classroom)
                .WithMany()
                .HasForeignKey(x => x.ClassroomId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.StudyGroupId);
            builder.HasIndex(x => x.ClassroomId);
            builder.HasIndex(x => x.Date);
        }
    }
}