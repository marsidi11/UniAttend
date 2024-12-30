using Microsoft.Extensions.Logging;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Application.Common.Exceptions;

namespace UniAttend.Infrastructure.Services
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILoggerService _logger;
        private readonly IEmailService _emailService;

        public ExceptionHandler(
            ILoggerService logger,
            IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task HandleExceptionAsync(Exception exception)
        {
            _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

            if (IsCriticalException(exception))
            {
                await NotifyAdministratorsAsync(exception);
            }
        }

        public bool IsCriticalException(Exception exception)
        {
            return exception switch
            {
                DatabaseException => true,
                SecurityException => true,
                SystemException => true,
                _ => false
            };
        }

        public string GetUserFriendlyMessage(Exception exception)
        {
            return exception switch
            {
                ValidationException validationEx => 
                    "Invalid data provided: " + string.Join(", ", validationEx.Errors),
                NotFoundException => "The requested resource was not found.",
                UnauthorizedAccessException => "You don't have permission to perform this action.",
                _ => "An unexpected error occurred. Please try again later."
            };
        }

        private async Task NotifyAdministratorsAsync(Exception exception)
        {
            var subject = $"Critical Error in UniAttend System - {DateTime.UtcNow}";
            var body = $"""
                A critical error occurred in the UniAttend system:
                
                Error Type: {exception.GetType().Name}
                Message: {exception.Message}
                Stack Trace: {exception.StackTrace}
                
                Time (UTC): {DateTime.UtcNow}
                """;

            await _emailService.SendEmailAsync("admin@uniattend.com", subject, body);
        }
    }
}