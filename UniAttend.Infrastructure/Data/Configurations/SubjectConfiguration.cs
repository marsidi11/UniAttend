using UniAttend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniAttend.Infrastructure.Data.Configurations
{
    internal class SubjectConfiguration : EntityConfiguration<Subject>
    {
        public override void Configure(EntityTypeBuilder<Subject> builder)
        {
            base.Configure(builder);

            builder.ToTable("Subjects");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.Credits)
                .IsRequired();

            builder.Property(x => x.DepartmentId)
                .IsRequired();

            // Relationship with Department
            builder.HasOne(x => x.Department)
                .WithMany(x => x.Subjects)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();                

            // Add unique constraint for name within department
            builder.HasIndex(x => new { x.Name, x.DepartmentId })
                .IsUnique();
        }
    }
}