using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Infrastructure.Settings;

namespace UniAttend.Infrastructure.Services
{
    /// <summary>
    /// Provides email sending functionalities.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="settings">Email configuration settings.</param>
        /// <param name="logger">Logger instance.</param>
        public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Sends an absence alert email to the specified recipient.
        /// </summary>
        /// <param name="email">Recipient's email address.</param>
        /// <param name="studentName">Name of the student.</param>
        /// <param name="courseName">Name of the course.</param>
        /// <param name="absencePercentage">Current absence percentage.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task SendAbsenceAlertAsync(
            string email, 
            string studentName,
            string courseName, 
            decimal absencePercentage,
            CancellationToken cancellationToken = default)
        {
            var subject = "Attendance Alert";
            var body = $"Dear {studentName},\n\n" +
                       $"Your attendance in {courseName} has fallen below the required threshold. " +
                       $"Current attendance: {absencePercentage:F1}%\n\n" +
                       "Please contact your professor or academic advisor.";

            await SendEmailAsync(email, subject, body, cancellationToken);
        }

        /// <summary>
        /// Sends an OTP code email for attendance.
        /// </summary>
        /// <param name="email">Recipient's email address.</param>
        /// <param name="otpCode">The OTP code.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="expiryTime">The time when the OTP expires.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task SendOtpCodeAsync(
            string email, 
            string otpCode,
            string className, 
            DateTime expiryTime,
            CancellationToken cancellationToken = default)
        {
            var subject = "Attendance OTP Code";
            var body = $"Your OTP code for {className} is: {otpCode}\n" +
                       $"This code will expire at: {expiryTime:g}";

            await SendEmailAsync(email, subject, body, cancellationToken);
        }

        /// <summary>
        /// Sends an email with the specified details.
        /// </summary>
        /// <param name="to">Recipient's email address.</param>
        /// <param name="subject">Email subject line.</param>
        /// <param name="body">Email body content.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task SendEmailAsync(
            string to, 
            string subject, 
            string body, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_settings.DisplayName, _settings.FromAddress));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;

                var builder = new BodyBuilder
                {
                    TextBody = body
                };
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls, cancellationToken);
                await smtp.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
                await smtp.SendAsync(email, cancellationToken);
                await smtp.DisconnectAsync(true, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", to);
                throw;
            }
        }

        /// <summary>
        /// Sends a welcome email with account details.
        /// </summary>
        /// <param name="email">Recipient's email address.</param>
        /// <param name="fullName">Full name of the recipient.</param>
        /// <param name="username">Username for the account.</param>
        /// <param name="temporaryPassword">Temporary password provided.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task SendWelcomeEmailAsync(
            string email, 
            string fullName, 
            string username,
            string temporaryPassword,
            CancellationToken cancellationToken = default)
        {
            var subject = "Welcome to UniAttend - Account Details";
            var body = $"""
                Dear {fullName},

                Welcome to UniAttend! Your account has been created successfully.

                Here are your login credentials:
                Username: {username}
                Temporary Password: {temporaryPassword}

                For security reasons, please change your password after your first login.

                Best regards,
                The UniAttend Team
                """;

            await SendEmailAsync(email, subject, body, cancellationToken);
        }

        /// <summary>
        /// Sends a password reset email with new credentials.
        /// </summary>
        /// <param name="email">Recipient's email address.</param>
        /// <param name="fullName">Full name of the recipient.</param>
        /// <param name="username">Username for the account.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task SendPasswordResetEmailAsync(
            string email,
            string fullName,
            string username,
            string newPassword,
            CancellationToken cancellationToken = default)
        {
            var subject = "UniAttend - Password Reset";
            var body = $"""
                Dear {fullName},

                Your password has been reset successfully.

                Your login credentials:
                Username: {username}
                New Password: {newPassword}

                For security reasons, please change your password after logging in.

                Best regards,
                The UniAttend Team
                """;

            await SendEmailAsync(email, subject, body, cancellationToken);
        }
    }
}