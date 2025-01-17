using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Identity;

namespace UniAttend.Infrastructure.Data.Configurations
{
        internal class StudentConfiguration : EntityConfiguration<Student>
    {
        public override void Configure(EntityTypeBuilder<Student> builder)
        {
            base.Configure(builder);
    
            builder.ToTable("Students");
    
            // Properties
            builder.Property(s => s.StudentId)
                .IsRequired()
                .HasMaxLength(20);
    
            builder.Property(s => s.CardId)
                .HasMaxLength(50);
    
            builder.Property(s => s.DepartmentId)
                .IsRequired();
    
            // Configure one-to-one relationship with User
            builder.HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.Id) 
                .OnDelete(DeleteBehavior.Restrict);
    
            // Configure relationship with Department
            builder.HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
    
            // Indexes
            builder.HasIndex(s => s.StudentId).IsUnique();
            builder.HasIndex(s => s.CardId)
                .IsUnique()
                .HasFilter("[CardId] IS NOT NULL");
            builder.HasIndex(s => s.DepartmentId);
        }
    }
}