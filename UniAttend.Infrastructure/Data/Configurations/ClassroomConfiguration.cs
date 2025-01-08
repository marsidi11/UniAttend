using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities;

namespace UniAttend.Infrastructure.Data.Configurations
{
    internal class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
    {
        public void Configure(EntityTypeBuilder<Classroom> builder)
        {
            builder.ToTable("Classrooms");
            
            builder.HasKey(x => x.Id);
            
            // Use backing fields for immutable properties
            builder.Property("Name")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property("ReaderDeviceId")
                .HasMaxLength(50);

            // Indexes for performance
            builder.HasIndex("Name")
                .IsUnique();
            
            builder.HasIndex("ReaderDeviceId")
                .IsUnique()
                .HasFilter("[ReaderDeviceId] IS NOT NULL");
        }
    }
}