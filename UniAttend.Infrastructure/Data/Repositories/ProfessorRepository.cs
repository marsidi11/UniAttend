using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;

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
    public class ProfessorRepository : IProfessorRepository
    {
        private readonly ApplicationDbContext _context;

        public ProfessorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Professor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Professors
                .Include(p => p.User)
                .Include(p => p.Department)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        public async Task<IEnumerable<Professor>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _context.Professors
                .Include(p => p.User)
                .Include(p => p.Department)
                .ToListAsync(cancellationToken);

        public async Task<Professor> AddAsync(Professor entity, CancellationToken cancellationToken = default)
        {
            await _context.Professors.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(Professor entity, CancellationToken cancellationToken = default)
        {
            _context.Professors.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var professor = await GetByIdAsync(id, cancellationToken);
            if (professor != null)
            {
                _context.Professors.Remove(professor);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Professors.AnyAsync(p => p.Id == id, cancellationToken);

        public async Task<Professor?> GetByUserId(int userId, CancellationToken cancellationToken = default)
            => await _context.Professors
                .Include(p => p.User)
                .Include(p => p.Department)
                .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        public async Task<IEnumerable<Professor>> GetByDepartmentId(int departmentId, CancellationToken cancellationToken = default)
            => await _context.Professors
                .Include(p => p.User)
                .Include(p => p.Department)
                .Where(p => p.DepartmentId == departmentId)
                .ToListAsync(cancellationToken);
    }
}