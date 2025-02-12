using UniAttend.Core.Enums;

namespace UniAttend.Application.Features.Users.DTOs
{
    /// <summary>
    /// Data transfer object representing detailed user profile information
    /// </summary>
    public record UserProfileDto(
        int Id,
        string Username,
        string Email,
        string FirstName,
        string LastName,
        UserRole Role,
        bool IsTwoFactorEnabled,
        bool IsTwoFactorVerified
    );
}