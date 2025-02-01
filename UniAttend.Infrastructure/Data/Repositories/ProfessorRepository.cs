using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Enums;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository for managing Professor entities.
    /// </summary>
    public class ProfessorRepository : BaseRepository<Professor>, IProfessorRepository
    {
        public ProfessorRepository(ApplicationDbContext context) : base(context) { }

        /// <summary>
        /// Gets all professor entities with details.
        /// </summary>
        public override async Task<IEnumerable<Professor>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(p => p.User)
                .Include(p => p.Departments)
                .Where(p => p.User != null && p.User.Role == UserRole.Professor)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a professor by its identifier.
        /// </summary>
        public override async Task<Professor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(p => p.User)
                .Include(p => p.Departments)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        /// <summary>
        /// Gets a professor by the associated user identifier.
        /// </summary>
        public async Task<Professor?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(p => p.User)
                .Include(p => p.Departments)
                .FirstOrDefaultAsync(p => p.Id == userId, cancellationToken);
        }

        /// <summary>
        /// Gets professors associated with a specific department.
        /// </summary>
        public async Task<IEnumerable<Professor>> GetByDepartmentId(int departmentId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(p => p.User)
                .Include(p => p.Departments)
                .Where(p => p.Departments.Any(d => d.Id == departmentId))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}