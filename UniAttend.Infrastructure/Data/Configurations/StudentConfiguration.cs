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

            builder.Property("StudentId")
                .IsRequired()
                .HasMaxLength(20);

            builder.Property("CardId")
                .HasMaxLength(50);

            builder.Property("DepartmentId")
                .IsRequired();

            // Configure relationships
            builder.HasOne<Department>()
                .WithMany(d => d.Students)
                .HasForeignKey("DepartmentId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                .WithOne()
                .HasForeignKey<Student>("UserId")
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for performance
            builder.HasIndex("StudentId").IsUnique();
            builder.HasIndex("CardId")
                .IsUnique()
                .HasFilter("[CardId] IS NOT NULL");
            builder.HasIndex("DepartmentId");
        }
    }
}