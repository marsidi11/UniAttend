using System.Security.Cryptography;
using UniAttend.Core.Interfaces.Services;

namespace UniAttend.Infrastructure.Services
{
    /// <summary>
    /// Provides secure password hashing and verification services using PBKDF2 (Password-Based Key Derivation Function 2).
    /// This implementation uses RFC2898 with configurable salt size, hash size, and iteration count for robust protection against brute-force and rainbow table attacks.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        public string HashPassword(string password)
        {
            ArgumentNullException.ThrowIfNull(password);
            
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            var pbkdf2 = new Rfc2898DeriveBytes(
                password, 
                salt, 
                Iterations, 
                HashAlgorithmName.SHA256);
                
            byte[] hash = pbkdf2.GetBytes(HashSize);

            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            ArgumentNullException.ThrowIfNull(password);
            ArgumentNullException.ThrowIfNull(hashedPassword);

            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var pbkdf2 = new Rfc2898DeriveBytes(
                password, 
                salt, 
                Iterations,
                HashAlgorithmName.SHA256);
                
            byte[] hash = pbkdf2.GetBytes(HashSize);

            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            }
            return true;
        }
    }
}