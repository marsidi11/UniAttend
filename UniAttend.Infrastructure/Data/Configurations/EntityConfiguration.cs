using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities.Base;

namespace UniAttend.Infrastructure.Data.Configurations
{
    public abstract class EntityConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {            
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder.Property(e => e.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .HasColumnType("datetime")
                .IsRequired(false);
        }
    }
}