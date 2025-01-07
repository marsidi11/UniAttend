using System;
using System.Threading.Tasks;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Infrastructure.Services
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILoggerService _logger;

        public ExceptionHandler(ILoggerService logger)
        {
            _logger = logger;
        }

        public string GetUserFriendlyMessage(Exception exception)
        {
            return exception switch
            {
                UnauthorizedException => "You are not authorized to perform this action.",
                ValidationException => exception.Message,
                NotFoundException => "The requested resource was not found.",
                _ => "An unexpected error occurred. Please try again later."
            };
        }

        public async Task HandleExceptionAsync(Exception exception)
        {
            await _logger.LogErrorAsync(exception.Message, exception);
        }

        // For backwards compatibility
        public Task<string> GetUserFriendlyMessageAsync(Exception exception)
        {
            return Task.FromResult(GetUserFriendlyMessage(exception));
        }
    }
}