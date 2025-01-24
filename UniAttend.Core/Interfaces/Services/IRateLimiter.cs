namespace UniAttend.Core.Interfaces.Services
{
    public interface IRateLimiter
    {
        Task<bool> CheckAsync(string key, int maxAttempts, TimeSpan window);
    }
}