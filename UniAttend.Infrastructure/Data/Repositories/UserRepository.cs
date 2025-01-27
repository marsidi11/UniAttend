using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Identity;
using UniAttend.Core.Enums;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository implementation for managing User entities in the database.
    /// Provides CRUD operations and user-specific queries.
    /// </summary>
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> AnyAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(predicate, cancellationToken);
        }

                public async Task<User> CreateUserWithRoleAsync(
            User user,
            UserRole role,
            IEnumerable<int>? departmentIds = null,
            CancellationToken cancellationToken = default)
        {
            if (role == UserRole.Professor && (departmentIds == null || !departmentIds.Any()))
                throw new ValidationException("At least one department is required for professors");
        
            await _context.Users.AddAsync(user, cancellationToken);
        
            if (role == UserRole.Professor)
            {
                var professor = new Professor(user);
                await _context.Professors.AddAsync(professor, cancellationToken);
        
                foreach (var departmentId in departmentIds!)
                {
                    var department = await _context.Departments
                        .FindAsync(new object[] { departmentId }, cancellationToken)
                        ?? throw new NotFoundException($"Department with ID {departmentId} not found");
                    
                    professor.AddDepartment(department);
                }
            }
        
            return user;
        }

        public override async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(u => u.Username == username, cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);
        }
    }
}