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
            base.Configure(builder);

            builder.Property(u => u.Username)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(u => u.Role)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.HasIndex(u => u.Username)
                .IsUnique();

            builder.HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}