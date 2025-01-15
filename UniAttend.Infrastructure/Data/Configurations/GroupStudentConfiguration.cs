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
            
            // Required properties
            builder.Property(x => x.GroupId)
                .IsRequired();
            
            builder.Property(x => x.StudentId)
                .IsRequired();

            // Relationships
            builder.HasOne(gs => gs.Group)
                .WithMany()
                .HasForeignKey(gs => gs.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(gs => gs.Student)
                .WithMany()
                .HasForeignKey(gs => gs.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint on GroupId and StudentId combination
            builder.HasIndex(gs => new { gs.GroupId, gs.StudentId })
                .IsUnique();

            // Table name
            builder.ToTable("GroupStudents");
        }
    }
}