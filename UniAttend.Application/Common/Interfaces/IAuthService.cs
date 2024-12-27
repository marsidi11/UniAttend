using UniAttend.Core.Entities.Identity;

/// <summary>
/// Service for authentication operations including token generation, validation, and password hashing.
/// </summary>
namespace UniAttend.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(User user);
        Task<bool> ValidateCredentialsAsync(string username, string password);
        string HashPassword(string password);
    }
}