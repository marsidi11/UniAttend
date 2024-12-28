using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UniAttend.Application.Common.Interfaces;
using UniAttend.Application.Auth.Common;
using UniAttend.Core.Entities.Identity;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;

        public AuthService(
            JwtSettings jwtSettings,
            IPasswordHasher passwordHasher,
            IUserRepository userRepository)
        {
            _jwtSettings = jwtSettings;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler()
                .WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            return (accessToken, refreshToken);
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(password);
        }

        public async Task<bool> ValidateCredentialsAsync(
            string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return false;

            return _passwordHasher.VerifyPassword(
                password, user.PasswordHash);
        }

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.RandomNumberGenerator
                    .GetBytes(64));
        }
    }
}