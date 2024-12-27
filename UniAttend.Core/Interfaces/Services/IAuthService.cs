using UniAttend.Core.Entities.Identity;

namespace UniAttend.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(string accessToken, string refreshToken)> GenerateTokensAsync(User user);
    }
}