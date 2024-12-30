using UniAttend.Core.Interfaces.Services;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using UniAttend.Infrastructure.Settings;
using Microsoft.Extensions.Logging;

namespace UniAttend.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendAbsenceAlertAsync(string email, string studentName, 
            string courseName, decimal absencePercentage, 
            CancellationToken cancellationToken = default)
        {
            var subject = "Attendance Alert";
            var body = $"Dear {studentName},\n\n" +
                      $"Your attendance in {courseName} has fallen below the required threshold. " +
                      $"Current attendance: {absencePercentage:F1}%\n\n" +
                      "Please contact your professor or academic advisor.";

            await SendEmailAsync(email, subject, body, cancellationToken);
        }

        public async Task SendOtpCodeAsync(string email, string otpCode, 
            string className, DateTime expiryTime, 
            CancellationToken cancellationToken = default)
        {
            var subject = "Attendance OTP Code";
            var body = $"Your OTP code for {className} is: {otpCode}\n" +
                      $"This code will expire at: {expiryTime:g}";

            await SendEmailAsync(email, subject, body, cancellationToken);
        }

        /// <summary>
        /// Sends an email using configured SMTP settings
        /// </summary>
        /// <param name="to">Recipient email address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body content</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task SendEmailAsync(string to, string subject, 
            string body, CancellationToken cancellationToken = default)
        {
            try
            {
                using var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
                {
                    EnableSsl = _settings.EnableSsl,
                    Credentials = new System.Net.NetworkCredential(_settings.Username, _settings.Password)
                };
                using var message = new MailMessage(_settings.FromAddress, to, subject, body);
                await client.SendMailAsync(message, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", to);
                throw;
            }
        }
    }
}