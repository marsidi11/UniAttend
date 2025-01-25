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

            builder.Property(x => x.DepartmentId)
                .IsRequired();

            // Professor shares the same Id with User (one-to-one)
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            // Configure many-to-many relationship with Department
            builder.HasMany(p => p.Departments)
                .WithMany(d => d.Professors)
                .UsingEntity(j => j.ToTable("ProfessorDepartments"));

            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<Professor>(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}