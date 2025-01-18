using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities.Audit;

namespace UniAttend.Infrastructure.Data.Configurations
{
    internal class AuditLogConfiguration : EntityConfiguration<AuditLog>
    {
        public override void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("AuditLogs");

            builder.Property(x => x.Action)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.EntityType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.EntityId)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Details)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.Timestamp)
                .IsRequired();

            // Index for faster querying
            builder.HasIndex(x => new { x.UserId, x.Timestamp });
        }
    }
}