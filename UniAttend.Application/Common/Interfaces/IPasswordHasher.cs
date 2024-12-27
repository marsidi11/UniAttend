/// <summary>
/// Provides interface for password hashing and verification operations.
/// Handles secure password management through hashing and validation.
/// </summary>
namespace UniAttend.Application.Common.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}