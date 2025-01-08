using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities;

namespace UniAttend.Infrastructure.Data.Configurations
{
    internal class StudyGroupConfiguration : IEntityTypeConfiguration<StudyGroup>
    {
        public void Configure(EntityTypeBuilder<StudyGroup> builder)
        {
            builder.ToTable("StudyGroups");
            
            builder.HasKey(x => x.Id);
            
            // Use backing fields for immutable properties
            builder.Property("Name")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property("SubjectId")
                .IsRequired();

            builder.Property("AcademicYearId")
                .IsRequired();

            builder.Property("ProfessorId")
                .IsRequired();

            builder.Property("IsActive")
                .IsRequired()
                .HasDefaultValue(true);

            // Configure relationships
            builder.HasOne(typeof(Subject))
                .WithMany()
                .HasForeignKey("SubjectId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(typeof(AcademicYear))
                .WithMany()
                .HasForeignKey("AcademicYearId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(typeof(Professor))
                .WithMany()
                .HasForeignKey("ProfessorId")
                .OnDelete(DeleteBehavior.Restrict);

            // Collections
            builder.HasMany(x => x.Students)
                .WithOne(x => x.Group)
                .HasForeignKey("GroupId")
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex("Name", "SubjectId", "AcademicYearId")
                .IsUnique();
        }
    }
}