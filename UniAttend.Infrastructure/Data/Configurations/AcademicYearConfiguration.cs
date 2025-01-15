using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities;

namespace UniAttend.Infrastructure.Data.Configurations
{
    internal class AcademicYearConfiguration : EntityConfiguration<AcademicYear>
    {
        public override void Configure(EntityTypeBuilder<AcademicYear> builder)
        {
            base.Configure(builder);

            builder.ToTable("AcademicYears"); 

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.EndDate)
                .IsRequired();

            // Configure relationships
            builder.HasMany(x => x.StudyGroups)
                .WithOne(x => x.AcademicYear)
                .HasForeignKey(x => x.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}