using System.Net;
using System.Text.Json;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Shared.Exceptions;

namespace UniAttend.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(
            RequestDelegate next,
            IExceptionHandler exceptionHandler,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _exceptionHandler = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)GetStatusCode(exception);

            var response = new
            {
                Status = context.Response.StatusCode,
                Message = await _exceptionHandler.GetUserFriendlyMessageAsync(exception),
                TraceId = context.TraceIdentifier,
                // Detailed error info in development
                // TODO: Remove before production
                Detail = context.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() == true
                    ? new
                    {
                        ExceptionMessage = exception.Message,
                        StackTrace = exception.StackTrace,
                        Source = exception.Source
                    }
                    : null
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }

        private static HttpStatusCode GetStatusCode(Exception exception) =>
            exception switch
            {
                UnauthorizedException => HttpStatusCode.Unauthorized,
                ValidationException => HttpStatusCode.BadRequest,
                NotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };
    }
}