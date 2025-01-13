using UniAttend.Core.Enums;

namespace UniAttend.Application.Features.Auth.DTOs
{
    /// <summary>
    /// Data transfer object representing user information with essential details for authentication and identification.
    /// </summary>
    public record UserAuthDto(
        int Id,
        string Username,
        string Email,
        string FirstName,
        string LastName,
        UserRole Role
    );
}