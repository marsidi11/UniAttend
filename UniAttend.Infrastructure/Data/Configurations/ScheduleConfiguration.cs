using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities;

namespace UniAttend.Infrastructure.Data.Configurations
{
    internal class ScheduleConfiguration : EntityConfiguration<Schedule>
    {
        public override void Configure(EntityTypeBuilder<Schedule> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("Schedules");
                        
            // Use backing fields for immutable properties
            builder.Property("GroupId")
                .IsRequired();

            builder.Property("ClassroomId")
                .IsRequired();

            builder.Property("DayOfWeek")
                .IsRequired();

            builder.Property("StartTime")
                .IsRequired();

            builder.Property("EndTime")
                .IsRequired();

            // Configure relationships using string names to avoid navigation property exposure
            builder.HasOne(typeof(StudyGroup))
                .WithMany()
                .HasForeignKey("GroupId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(typeof(Classroom))
                .WithMany()
                .HasForeignKey("ClassroomId")
                .OnDelete(DeleteBehavior.Restrict);

            // Index for performance
            builder.HasIndex("ClassroomId", "DayOfWeek");
        }
    }
}