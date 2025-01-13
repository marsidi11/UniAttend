using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using UniAttend.Application.Common.Exceptions;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Infrastructure.Auth.Settings;

namespace UniAttend.API.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSettings _jwtSettings;
        private readonly IAuthService _authService;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(
            RequestDelegate next, 
            JwtSettings jwtSettings,
            IAuthService authService,
            ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _jwtSettings = jwtSettings;
            _authService = authService;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await AttachUserToContext(context, token);

            await _next(context);
        }

        private async Task AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "sub").Value);

                context.Items["UserId"] = userId;
            }
            catch (SecurityTokenExpiredException)
            {
                _logger.LogWarning("Token validation failed: Token expired");
                throw new SecurityException("Token has expired");
            }
            catch (SecurityTokenValidationException ex)
            {
                _logger.LogWarning(ex, "Token validation failed: Invalid token");
                throw new SecurityException("Invalid token");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the JWT token");
                throw new SecurityException("Token validation failed");
            }
        }
    }
}