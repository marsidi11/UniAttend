using UniAttend.Core.Entities.Attendance;

/// <summary>
/// Service responsible for generating and validating One-Time Password (OTP) codes for student attendance
/// </summary>
namespace UniAttend.Core.Interfaces.Services
{
    public interface IOtpService
    {
        Task<OtpCode> GenerateOtpAsync(int classId, int studentId, CancellationToken cancellationToken = default);
        Task<bool> ValidateOtpAsync(string code, int classId, int studentId, CancellationToken cancellationToken = default);
        Task<OtpCode?> GetCurrentOtpAsync(int classId, CancellationToken cancellationToken = default);
    }
}