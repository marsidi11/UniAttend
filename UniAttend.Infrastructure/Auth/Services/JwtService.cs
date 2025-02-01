using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Infrastructure.Auth.Settings;

namespace UniAttend.Infrastructure.Auth.Services
{
    /// <summary>
    /// Provides functionalities for generating and validating JWT tokens.
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtService"/> class.
        /// </summary>
        /// <param name="jwtSettings">The JWT configuration settings.</param>
        /// <exception cref="ArgumentNullException">Thrown if jwtSettings is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if required JWT settings (Key, Issuer, Audience) are missing.
        /// </exception>
        public JwtService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings ?? throw new ArgumentNullException(nameof(jwtSettings));

            if (string.IsNullOrEmpty(jwtSettings.Key))
                throw new InvalidOperationException("JWT:Key is not configured in appsettings.json");

            if (string.IsNullOrEmpty(jwtSettings.Issuer))
                throw new InvalidOperationException("JWT:Issuer is not configured in appsettings.json");

            if (string.IsNullOrEmpty(jwtSettings.Audience))
                throw new InvalidOperationException("JWT:Audience is not configured in appsettings.json");
        }

        /// <summary>
        /// Generates a JWT token with the specified claims.
        /// </summary>
        /// <param name="claims">A collection of claims to include in the token.</param>
        /// <returns>A signed JWT token string.</returns>
        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Validates the specified JWT token.
        /// </summary>
        /// <param name="token">The JWT token string to validate.</param>
        /// <returns>A <see cref="ClaimsPrincipal"/> extracted from the validated token.</returns>
        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ClockSkew = TimeSpan.Zero
            };

            return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
        }

        /// <summary>
        /// Generates a secure random refresh token.
        /// </summary>
        /// <returns>A new refresh token as a base64 encoded string.</returns>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Extracts the claims principal from an expired JWT token.
        /// </summary>
        /// <param name="token">The expired JWT token string.</param>
        /// <returns>A <see cref="ClaimsPrincipal"/> extracted from the expired token.</returns>
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
        }
    }
}