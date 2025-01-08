using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities.Attendance;

namespace UniAttend.Infrastructure.Data.Configurations
{
    internal class OtpCodeConfiguration : IEntityTypeConfiguration<OtpCode>
    {
        public void Configure(EntityTypeBuilder<OtpCode> builder)
        {
            builder.ToTable("OtpCodes");
            
            builder.HasKey(x => x.Id);
            
            // Use backing fields for immutable properties
            builder.Property("StudentId").IsRequired();
            builder.Property("ClassId").IsRequired();
            builder.Property("Code")
                .IsRequired()
                .HasMaxLength(6);
            builder.Property("ExpiryTime").IsRequired();
            builder.Property("IsUsed")
                .IsRequired()
                .HasDefaultValue(false);

            // Indexes for fast lookups
            builder.HasIndex("Code", "ClassId", "StudentId")
                .IsUnique();
            builder.HasIndex("ClassId", "ExpiryTime");

            // Enforce code format
            builder.HasCheckConstraint("CK_OtpCode_Format", "LEN([Code]) = 6 AND [Code] NOT LIKE '%[^0-9]%'");
        }
    }
}