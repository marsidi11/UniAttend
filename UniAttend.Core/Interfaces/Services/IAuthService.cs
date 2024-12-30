using UniAttend.Core.Entities.Identity;

namespace UniAttend.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(User user);
        Task<bool> ValidateCredentialsAsync(string username, string password);
        string HashPassword(string password);
    }
}