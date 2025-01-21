using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Course?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(c => c.Professor)
                .Include(c => c.StudyGroup)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<IEnumerable<Course>> GetByGroupIdAsync(int studyGroupId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(c => c.Professor)
                .Include(c => c.StudyGroup)
                .Where(c => c.StudyGroupId == studyGroupId)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Course>> GetByProfessorIdAsync(int professorId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(c => c.Professor)
                .Include(c => c.StudyGroup)
                .Where(c => c.ProfessorId == professorId)
                .ToListAsync(cancellationToken);
    }
}