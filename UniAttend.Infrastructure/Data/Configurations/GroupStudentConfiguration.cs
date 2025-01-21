using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities;

namespace UniAttend.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configuration class for GroupStudent entity defining the database schema and relationships.
    /// This class maps the GroupStudent entity to the database using Entity Framework Core configurations.
    /// </summary>
    public class GroupStudentConfiguration : EntityConfiguration<GroupStudent>
    {
        public override void Configure(EntityTypeBuilder<GroupStudent> builder)
        {
            base.Configure(builder);

            builder.ToTable("GroupStudents");

            // Required properties
            builder.Property(x => x.StudyGroupId)
                .IsRequired();

            builder.Property(x => x.StudentId)
                .IsRequired();

            // Single relationship to StudyGroup
            builder.HasOne(gs => gs.StudyGroup)
                .WithMany(g => g.Students)
                .HasForeignKey(gs => gs.StudyGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(gs => gs.Student)
                .WithMany()
                .HasForeignKey(gs => gs.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint on StudyGroupId and StudentId combination
            builder.HasIndex(gs => new { gs.StudyGroupId, gs.StudentId })
                .IsUnique();
        }
    }
}