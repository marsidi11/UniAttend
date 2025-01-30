using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities.Attendance;

namespace UniAttend.Infrastructure.Data.Configurations
{
    internal class AttendanceRecordConfiguration : EntityConfiguration<AttendanceRecord>
    {
        public override void Configure(EntityTypeBuilder<AttendanceRecord> builder)
        {
            base.Configure(builder);

            builder.ToTable("AttendanceRecords");

            // Properties
            builder.Property(x => x.CourseSessionId).IsRequired();
        builder.Property(x => x.StudentId).IsRequired();
        builder.Property(x => x.CheckInTime).IsRequired();
        builder.Property(x => x.CheckInMethod)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();

            // Relationships
            builder.HasOne(x => x.CourseSession)
            .WithMany(cs => cs.AttendanceRecords)
            .HasForeignKey(x => x.CourseSessionId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}