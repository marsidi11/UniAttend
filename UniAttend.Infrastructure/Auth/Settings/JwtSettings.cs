/// <summary>
/// Contains JSON Web Token (JWT) configuration settings used for authentication and authorization.
/// It includes essential properties required for JWT token generation and validation.
/// </summary>
namespace UniAttend.Infrastructure.Auth.Settings
{
    public class JwtSettings
    {
        /// <summary>
        /// Gets the secret key used for signing tokens.
        /// </summary>
        public string Key { get; init; } = string.Empty;

        /// <summary>
        /// Gets the issuer of the token.
        /// </summary>
        public string Issuer { get; init; } = string.Empty;

        /// <summary>
        /// Gets the audience for the token.
        /// </summary>
        public string Audience { get; init; } = string.Empty;

        /// <summary>
        /// Gets the token expiration time (in minutes).
        /// </summary>
        public int TokenExpirationInMinutes { get; init; } = 60;
    }
}