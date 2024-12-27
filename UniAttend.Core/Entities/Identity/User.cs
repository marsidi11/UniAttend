using UniAttend.Core.Entities.Base;
using UniAttend.Core.Enums;
using System.Text.RegularExpressions;

namespace UniAttend.Core.Entities.Identity
{
    /// <summary>
    /// Represents a user in the system with authentication and role-based access
    /// </summary>
    public class User : ActiveEntity
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        protected User() { }

        public User(string username, string passwordHash, string email, 
            UserRole role, string firstName, string lastName)
        {
            ValidateUsername(username);
            ValidateEmail(email);
            ValidateName(firstName, nameof(firstName));
            ValidateName(lastName, nameof(lastName));

            Username = username.Trim();
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            Email = email.Trim().ToLower();
            Role = role;
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }

        public string Username { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public UserRole Role { get; private set; }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public DateTime? LastLoginDate { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiryTime { get; private set; }

        public void UpdatePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash ?? throw new ArgumentNullException(nameof(newPasswordHash));
        }

        public void UpdateRefreshToken(string refreshToken, DateTime expiryTime)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = expiryTime;
        }

        public void RecordLogin() => LastLoginDate = DateTime.UtcNow;

        private static void ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));
            if (username.Length is < 3 or > 50)
                throw new ArgumentException("Username must be between 3 and 50 characters", nameof(username));
        }

        private static void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !EmailRegex.IsMatch(email))
                throw new ArgumentException("Invalid email format", nameof(email));
        }

        private static void ValidateName(string name, string paramName)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
                throw new ArgumentException($"Invalid {paramName}", paramName);
        }
    }
}