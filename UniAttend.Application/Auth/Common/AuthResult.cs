using UniAttend.Core.Enums;

/// <summary>
/// Contains authentication-related data transfer objects for managing user authentication results
/// and user information within the UniAttend application's domain.
/// </summary>
/// <remarks>
/// This namespace provides record types that encapsulate authentication results and user details,
/// adhering to Domain-Driven Design principles by maintaining clear boundaries between the
/// authentication domain and other parts of the application.
/// 
/// The types defined here serve as immutable data transfer objects, ensuring thread-safety
/// and maintaining data integrity throughout the authentication process.
/// </remarks>
namespace UniAttend.Application.Auth.Common
{
    public record AuthResult(
        string AccessToken,
        string RefreshToken,
        DateTime ExpiresAt,
        UserDto User);

    public record UserDto(
        int Id,
        string Username,
        string Email,
        string FirstName,
        string LastName,
        UserRole Role);
}