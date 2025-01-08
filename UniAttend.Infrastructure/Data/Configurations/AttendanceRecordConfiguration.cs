using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities.Attendance;

namespace UniAttend.Infrastructure.Data.Configurations
{
    internal class AttendanceRecordConfiguration : IEntityTypeConfiguration<AttendanceRecord>
    {
        public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
        {
            builder.ToTable("AttendanceRecords");
            
            builder.HasKey(x => x.Id);
            
            // Use backing fields for immutable properties
            builder.Property("CourseId").IsRequired();
            builder.Property("StudentId").IsRequired();
            builder.Property("CheckInTime").IsRequired();
            builder.Property("CheckInMethod").IsRequired().HasMaxLength(50);
            builder.Property("IsConfirmed").IsRequired();

            // Configure relationships
            builder.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey("CourseId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey("StudentId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}