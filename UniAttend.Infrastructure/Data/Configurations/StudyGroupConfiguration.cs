using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities;

namespace UniAttend.Infrastructure.Data.Configurations
{
        internal class StudyGroupConfiguration : EntityConfiguration<StudyGroup>
    {
        public override void Configure(EntityTypeBuilder<StudyGroup> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("StudyGroups");
            
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
        
            builder.Property(x => x.SubjectId)
                .IsRequired();
        
            builder.Property(x => x.AcademicYearId)
                .IsRequired();
        
            builder.Property(x => x.ProfessorId)
                .IsRequired();
        
            builder.HasOne(x => x.Subject)
                .WithMany(x => x.StudyGroups)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        
            builder.HasOne(x => x.AcademicYear)
                .WithMany(x => x.StudyGroups)
                .HasForeignKey(x => x.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);
        
            builder.HasOne(x => x.Professor)
                .WithMany()
                .HasForeignKey(x => x.ProfessorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.CourseSessions)
            .WithOne(x => x.StudyGroup)
            .HasForeignKey(x => x.StudyGroupId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}