using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

/// <summary>
/// Repository class for managing Professor entities in the database.
/// Implements the IProfessorRepository interface to provide CRUD operations and specialized queries for Professor data.
/// </summary>
/// <remarks>
/// This repository provides methods to:
/// - Retrieve professors by various criteria (ID, UserID, DepartmentID)
/// - Add new professors
/// - Update existing professor records
/// - Delete professors
/// - Check existence of professors
/// - Get all professors
/// All operations are asynchronous and support cancellation tokens.
/// </remarks>
namespace UniAttend.Infrastructure.Data.Repositories
{
        public class ProfessorRepository : BaseRepository<Professor>, IProfessorRepository
    {
        public ProfessorRepository(ApplicationDbContext context) : base(context) { }
    
        public override async Task<Professor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(p => p.User)
                .Include(p => p.Department)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        public async Task<Professor?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(p => p.User)
                .Include(p => p.Department)
                .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
        }

        public async Task<Professor?> GetByUserId(int userId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(p => p.User)
                .Include(p => p.Department)
                .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
    
        public async Task<IEnumerable<Professor>> GetByDepartmentId(int departmentId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(p => p.User)
                .Include(p => p.Department)
                .Where(p => p.DepartmentId == departmentId)
                .ToListAsync(cancellationToken);
    }
}