using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniAttend.Core.Entities.Attendance;

namespace UniAttend.Infrastructure.Data.Configurations
{
        internal class OtpCodeConfiguration : EntityConfiguration<OtpCode>
    {
        public override void Configure(EntityTypeBuilder<OtpCode> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("OtpCodes");
            
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
    
            // MySQL compatible check constraint
            builder.HasCheckConstraint("CK_OtpCode_Format", 
                "LENGTH(Code) = 6 AND Code REGEXP '^[0-9]+$'");
        }
    }
}