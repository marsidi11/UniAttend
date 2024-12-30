namespace UniAttend.Core.Interfaces.Services
{
    public interface IExceptionHandler
    {
        Task HandleExceptionAsync(Exception exception);
        bool IsCriticalException(Exception exception);
        string GetUserFriendlyMessage(Exception exception);
    }
}