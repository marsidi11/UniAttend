using System.Security.Claims;

namespace UniAttend.Core.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(IEnumerable<Claim> claims);
        ClaimsPrincipal ValidateToken(string token);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}