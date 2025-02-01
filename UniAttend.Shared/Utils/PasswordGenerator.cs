using System.Security.Cryptography;

namespace UniAttend.Shared.Utils
{
    /// <summary>
    /// Provides utilities for password generation.
    /// </summary>
    public static class PasswordGenerator
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
        private const int Length = 12;

        /// <summary>
        /// Generates a temporary password using a cryptographic random number generator.
        /// </summary>
        /// <returns>A temporary password string.</returns>
        public static string GenerateTemporaryPassword()
        {
            var bytes = new byte[Length];
            RandomNumberGenerator.Fill(bytes);

            var chars = new char[Length];
            for (int i = 0; i < Length; i++)
            {
                chars[i] = Chars[bytes[i] % Chars.Length];
            }

            return new string(chars);
        }
    }
}