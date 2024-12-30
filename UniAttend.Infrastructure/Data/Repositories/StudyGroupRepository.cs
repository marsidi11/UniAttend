using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository for managing study groups in the database.
    /// Provides CRUD operations and specific queries for StudyGroup entities.
    /// </summary>
    public class StudyGroupRepository : BaseRepository<StudyGroup>, IStudyGroupRepository
    {
        public StudyGroupRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<StudyGroup?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(g => g.Subject)
                .Include(g => g.Professor)
                .Include(g => g.AcademicYear)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        public override async Task<IEnumerable<StudyGroup>> GetAllAsync(CancellationToken cancellationToken = default)
            => await DbSet
                .Include(g => g.Subject)
                .Include(g => g.Professor)
                .Include(g => g.AcademicYear)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<StudyGroup>> GetBySubjectIdAsync(int subjectId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(g => g.Professor)
                .Include(g => g.AcademicYear)
                .Where(g => g.SubjectId == subjectId)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<StudyGroup>> GetByProfessorIdAsync(int professorId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(g => g.Subject)
                .Include(g => g.AcademicYear)
                .Where(g => g.ProfessorId == professorId)
                .ToListAsync(cancellationToken);
    }
}