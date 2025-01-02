using UniAttend.Core.Interfaces.Services;
using UniAttend.Shared.Exceptions;
using System.Net;
using System.Text.Json;

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
            _next = next;
            _exceptionHandler = exceptionHandler;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            await _exceptionHandler.HandleExceptionAsync(exception);

            var response = new
            {
                Status = (int)GetStatusCode(exception),
                Message = _exceptionHandler.GetUserFriendlyMessage(exception)
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.Status;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
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