/// <summary>
/// Contains JSON Web Token (JWT) configuration settings used for authentication and authorization.
/// It includes essential properties required for JWT token generation and validation.
/// </summary>
namespace UniAttend.Infrastructure.Auth.Settings
{
    public class JwtSettings
    {
        public string Key { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int TokenExpirationInMinutes { get; init; } = 60;
    }
}