using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities;

namespace UniAttend.Infrastructure.Data.Configurations
{
    internal class ClassroomConfiguration : EntityConfiguration<Classroom>
    {
        public override void Configure(EntityTypeBuilder<Classroom> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("Classrooms");
                        
            // Use backing fields for immutable properties
            builder.Property("Name")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property("ReaderDeviceId")
                .IsRequired(false)
                .HasMaxLength(50);
                
            // Indexes for performance
            builder.HasIndex("Name")
                .IsUnique();
        }
    }
}