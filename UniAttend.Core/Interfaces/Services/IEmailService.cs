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
            
        Task SendEmailAsync(string to, string subject, string body, 
            CancellationToken cancellationToken = default);

        Task SendWelcomeEmailAsync(string email, string fullName, string username, 
        string temporaryPassword, CancellationToken cancellationToken = default);

        Task SendPasswordResetEmailAsync(string email, string fullName, string username, 
        string newPassword, CancellationToken cancellationToken = default);
    }
}