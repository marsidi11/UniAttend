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
            
            builder.Property(x => x.StudyGroupId)
                .IsRequired();
        
            builder.Property(x => x.ClassroomId)
                .IsRequired();
        
            builder.Property(x => x.DayOfWeek)
                .IsRequired();
        
            builder.Property(x => x.StartTime)
                .IsRequired();
        
            builder.Property(x => x.EndTime)
                .IsRequired();
        
            // Configure single relationship to StudyGroup
            builder.HasOne(x => x.StudyGroup)
                .WithMany(x => x.Schedules)
                .HasForeignKey(x => x.StudyGroupId)
                .OnDelete(DeleteBehavior.Restrict);
        
            builder.HasOne(x => x.Classroom)
                .WithMany()
                .HasForeignKey(x => x.ClassroomId)
                .OnDelete(DeleteBehavior.Restrict);
        
            builder.HasIndex(x => new { x.ClassroomId, x.DayOfWeek });
        }
    }
}