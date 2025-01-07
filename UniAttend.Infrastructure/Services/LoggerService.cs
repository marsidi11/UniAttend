using Microsoft.Extensions.Logging;
using UniAttend.Core.Interfaces.Services;

namespace UniAttend.Infrastructure.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message, params object[] args) 
            => _logger.LogInformation(message, args);

        public void LogWarning(string message, params object[] args)
            => _logger.LogWarning(message, args);

        public void LogError(Exception exception, string message, params object[] args)
            => _logger.LogError(exception, message, args);

        public void LogDebug(string message, params object[] args)
            => _logger.LogDebug(message, args);

        public Task LogErrorAsync(string message, Exception exception)
        {
            LogError(exception, message);
            return Task.CompletedTask;
        }
    }
}