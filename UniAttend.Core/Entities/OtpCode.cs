using UniAttend.Core.Entities.Base;
using UniAttend.Core.Exceptions;

namespace UniAttend.Core.Entities.Attendance
{
    public class OtpCode : Entity
{
    private OtpCode() { } // For domain events/serialization

    public OtpCode(int studentId, int classId, string code, DateTime expiryTime)
    {
        ValidateCode(code);
        ValidateExpiryTime(expiryTime);
        
        StudentId = studentId;
        ClassId = classId;
        Code = code;
        ExpiryTime = expiryTime;
        IsUsed = false;
    }

    // Identity properties - immutable
    public int StudentId { get; }
    public int ClassId { get; }
    public string Code { get; } // Remove required keyword, use constructor initialization
    public DateTime ExpiryTime { get; }
    public bool IsUsed { get; private set; }

        // Domain methods
        public void MarkAsUsed() => IsUsed = true;

        public bool IsValid() => 
            !IsUsed && ExpiryTime > DateTime.UtcNow;

        // Domain validation
        private void ValidateCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new DomainException("OTP code cannot be empty");
            
            if (code.Length != 6 || !code.All(char.IsDigit))
                throw new DomainException("OTP code must be 6 digits");
        }

        private void ValidateExpiryTime(DateTime expiryTime)
        {
            if (expiryTime <= DateTime.UtcNow)
                throw new DomainException("Expiry time must be in the future");
        }
    }
}