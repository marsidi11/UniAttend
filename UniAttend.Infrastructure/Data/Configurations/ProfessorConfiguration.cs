using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Identity;

namespace UniAttend.Infrastructure.Data.Configurations
{
    internal class ProfessorConfiguration : IEntityTypeConfiguration<Professor>
    {
        public void Configure(EntityTypeBuilder<Professor> builder)
        {
            builder.ToTable("Professors");
            
            builder.HasKey(x => x.Id);
            
            // Use backing fields for immutable properties
            builder.Property("DepartmentId").IsRequired();
            builder.Property("UserId").IsRequired();

            // Configure relationships
            builder.HasOne<Department>()
                .WithMany(d => d.Professors)
                .HasForeignKey("DepartmentId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                .WithOne()
                .HasForeignKey<Professor>("UserId")
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for performance
            builder.HasIndex("DepartmentId", "UserId");
        }
    }
}