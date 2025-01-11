namespace UniAttend.Application.Auth.Common
{
    /// <summary>
    /// Represents the result of an authentication operation, containing tokens and user information.
    /// </summary>
    public record AuthResult(
        string AccessToken,
        string RefreshToken,
        DateTime ExpiresAt,
        UserAuthDto User
    );
}