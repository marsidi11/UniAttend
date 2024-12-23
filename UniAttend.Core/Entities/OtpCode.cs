using UniAttend.Core.Entities.Base;
using System;

namespace UniAttend.Core.Entities.Attendance
{
    public class OtpCode : Entity
    {
        public OtpCode(int studentId, int classId, string code, DateTime expiryTime)
        {
            StudentId = studentId;
            ClassId = classId;
            Code = code ?? throw new ArgumentNullException(nameof(code));
            ExpiryTime = expiryTime;
        }
        public int StudentId { get; }
        public int ClassId { get; }
        public string Code { get; }
        public DateTime ExpiryTime { get; }
        public bool IsUsed { get; set; } = false;
    }
}