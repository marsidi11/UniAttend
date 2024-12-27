using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository for managing study groups in the database.
    /// Provides CRUD operations and specific queries for StudyGroup entities.
    /// </summary>
    public class StudyGroupRepository : IStudyGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public StudyGroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StudyGroup?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _context.StudyGroups
                .Include(g => g.Subject)
                .Include(g => g.Professor)
                .Include(g => g.AcademicYear)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        public async Task<IEnumerable<StudyGroup>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _context.StudyGroups
                .Include(g => g.Subject)
                .Include(g => g.Professor)
                .Include(g => g.AcademicYear)
                .ToListAsync(cancellationToken);

        public async Task<StudyGroup> AddAsync(StudyGroup entity, CancellationToken cancellationToken = default)
        {
            await _context.StudyGroups.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(StudyGroup entity, CancellationToken cancellationToken = default)
        {
            _context.StudyGroups.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var group = await GetByIdAsync(id, cancellationToken);
            if (group != null)
            {
                _context.StudyGroups.Remove(group);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
            => await _context.StudyGroups.AnyAsync(g => g.Id == id, cancellationToken);

        public async Task<IEnumerable<StudyGroup>> GetBySubjectIdAsync(int subjectId, CancellationToken cancellationToken = default)
            => await _context.StudyGroups
                .Include(g => g.Professor)
                .Include(g => g.AcademicYear)
                .Where(g => g.SubjectId == subjectId)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<StudyGroup>> GetByProfessorIdAsync(int professorId, CancellationToken cancellationToken = default)
            => await _context.StudyGroups
                .Include(g => g.Subject)
                .Include(g => g.AcademicYear)
                .Where(g => g.ProfessorId == professorId)
                .ToListAsync(cancellationToken);
    }
}