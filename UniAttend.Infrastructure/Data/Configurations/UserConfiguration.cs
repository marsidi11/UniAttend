using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities.Identity;
using UniAttend.Core.Entities;

namespace UniAttend.Infrastructure.Data.Configurations
{
    internal class UserConfiguration : EntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            // Call base configuration first to set up Id, CreatedAt, UpdatedAt
            base.Configure(builder);

            // Entity specific configurations
            builder.Property(u => u.Username).HasMaxLength(50).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(255).IsRequired();
            builder.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(u => u.LastName).HasMaxLength(100).IsRequired();

            // Configure one-to-one relationships
            builder.HasOne(u => u.Professor)
                .WithOne(p => p.User)
                .HasForeignKey<Professor>("UserId");

            builder.HasOne(u => u.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>("UserId");

            builder.HasIndex(u => u.Username).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}