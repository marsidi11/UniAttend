namespace UniAttend.Core.Interfaces.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}