using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using UniAttend.Core.Entities.Identity;

namespace UniAttend.Core.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for managing User entities.
    /// Extends the generic repository interface with user-specific operations.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> AnyAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Retrieves a user by their unique username
        /// </summary>
        Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a user by their unique email address
        /// </summary>
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a username is already taken
        /// </summary>
        Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an email is already registered
        /// </summary>
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a user by their refresh token
        /// </summary>
        Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}