using System.Security.Claims;

namespace UniAttend.API.Extensions
{
    public static class ClaimsPrincipalExtensions 
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(ClaimTypes.NameIdentifier) 
                ?? principal.FindFirst("sub");

            if (claim == null)
                throw new InvalidOperationException("User ID claim not found");

            if (!int.TryParse(claim.Value, out int userId))
                throw new InvalidOperationException("Invalid user ID format");

            return userId;
        }

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(ClaimTypes.Email);

            if (claim == null)
                throw new InvalidOperationException("Email claim not found");

            return claim.Value;
        }
    }
}