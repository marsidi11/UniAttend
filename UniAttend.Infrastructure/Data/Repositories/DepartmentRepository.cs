using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository for managing Department entities.
    /// </summary>
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public DepartmentRepository(ApplicationDbContext context) : base(context) { }

        /// <summary>
        /// Retrieves a Department by its identifier with active subjects, students, and professors.
        /// </summary>
        /// <param name="id">The department identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A matching Department or null.</returns>
        public override async Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AsNoTracking()
                .Include(d => d.Subjects.Where(s => s.IsActive))
                .Include(d => d.Students)
                .Include(d => d.Professors)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        /// <summary>
        /// Retrieves a Department by its name with active subjects, students, and professors.
        /// </summary>
        /// <param name="name">The department name.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A matching Department or null.</returns>
        public async Task<Department?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AsNoTracking()
                .Include(d => d.Subjects.Where(s => s.IsActive))
                .Include(d => d.Students)
                .Include(d => d.Professors)
                .FirstOrDefaultAsync(d => d.Name == name, cancellationToken);
        }
    
        /// <summary>
        /// Gets all active Departments with active subjects, students, and professors.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of active Departments.</returns>
        public async Task<IEnumerable<Department>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(d => d.IsActive)
                .Include(d => d.Subjects.Where(s => s.IsActive))
                .Include(d => d.Students)
                .Include(d => d.Professors)
                .ToListAsync(cancellationToken);
        }
    
        /// <summary>
        /// Checks if a department with the specified name exists.
        /// </summary>
        /// <param name="name">The department name.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if the name exists; otherwise, false.</returns>
        public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(d => d.Name == name, cancellationToken);
        }
    }
}