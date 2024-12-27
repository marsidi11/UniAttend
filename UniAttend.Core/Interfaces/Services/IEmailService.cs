using UniAttend.Core.Entities;

/// <summary>
/// Service responsible for sending email notifications related to attendance and authentication.
/// </summary>
namespace UniAttend.Core.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAbsenceAlertAsync(string email, string studentName, string courseName, 
            decimal absencePercentage, CancellationToken cancellationToken = default);
        Task SendOtpCodeAsync(string email, string otpCode, string className, 
            DateTime expiryTime, CancellationToken cancellationToken = default);
    }
}