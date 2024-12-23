using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities.Identity
{
    public enum UserRole
    {
        Admin,
        Secretary,
        Professor,
        Student
    }

    public class User : Entity
    {
        public User(string username, string passwordHash, string email, UserRole role, string firstName, string lastName)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Role = role;
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        }

        public string Username { get; }
        public string PasswordHash { get; }
        public string Email { get; }
        public UserRole Role { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public Student? Student { get; set; }
        public Professor? Professor { get; set; }
    }
}