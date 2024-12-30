using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Entities.Identity;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;

        public AuthService(
            IJwtService jwtService,
            IPasswordHasher passwordHasher,
            IUserRepository userRepository)
        {
            _jwtService = jwtService;
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

            var accessToken = _jwtService.GenerateToken(claims);
            var refreshToken = _jwtService.GenerateRefreshToken();

            return (accessToken, refreshToken);
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(password);
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return false;

            return _passwordHasher.VerifyPassword(password, user.PasswordHash);
        }
    }
}