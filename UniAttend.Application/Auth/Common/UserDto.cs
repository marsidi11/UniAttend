using UniAttend.Core.Enums;

namespace UniAttend.Application.Auth.Common
{
    /// <summary>
    /// Data transfer object representing user information with essential details for authentication and identification.
    /// </summary>
    public record UserDto(
        int Id,
        string Username,
        string Email,
        string FirstName,
        string LastName,
        UserRole Role
    );
}