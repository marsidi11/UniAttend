using UniAttend.Core.Enums;

namespace UniAttend.Application.Auth.Common
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
        DateTime? LastLoginDate
    );
}