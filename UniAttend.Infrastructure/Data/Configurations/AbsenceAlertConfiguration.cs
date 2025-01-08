using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities;

namespace UniAttend.Infrastructure.Data.Configurations
{
    internal class AbsenceAlertConfiguration : IEntityTypeConfiguration<AbsenceAlert>
    {
        public void Configure(EntityTypeBuilder<AbsenceAlert> builder)
        {
            builder.ToTable("AbsenceAlerts");
            
            builder.HasKey(x => x.Id);
            
            // Use backing fields for immutable properties
            builder.Property("StudentId")
                .IsRequired();

            builder.Property("GroupId")
                .IsRequired();

            builder.Property("AbsencePercentage")
                .IsRequired()
                .HasPrecision(5, 2);

            builder.Property("EmailSent")
                .IsRequired()
                .HasDefaultValue(false);

            // Configure relationships
            builder.HasOne(typeof(Student))
                .WithMany()
                .HasForeignKey("StudentId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(typeof(StudyGroup))
                .WithMany()
                .HasForeignKey("GroupId")
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for performance
            builder.HasIndex("StudentId", "GroupId");
            builder.HasIndex("EmailSent");
        }
    }
}