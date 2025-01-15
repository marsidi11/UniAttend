using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Identity;

namespace UniAttend.Infrastructure.Data.Configurations
{
        internal class ProfessorConfiguration : EntityConfiguration<Professor>
        {
            public override void Configure(EntityTypeBuilder<Professor> builder)
            {
                base.Configure(builder);
        
                builder.ToTable("Professors");
                    
                // Configure primary key to match Users table
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Id)
                    .ValueGeneratedNever();
        
                builder.Property(p => p.DepartmentId)
                    .IsRequired();
        
                // Configure relationships
                builder.HasOne(p => p.Department)
                    .WithMany(d => d.Professors)
                    .HasForeignKey(p => p.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
        
                builder.HasOne(p => p.User)
                    .WithOne(u => u.Professor)
                    .HasForeignKey<Professor>(p => p.Id)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }
}