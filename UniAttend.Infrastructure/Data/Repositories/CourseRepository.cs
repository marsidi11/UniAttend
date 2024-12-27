using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities; 
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Infrastructure.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Course?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Courses
                .Include(c => c.Professor)
                .Include(c => c.StudyGroup)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _context.Courses
                .Include(c => c.Professor)
                .Include(c => c.StudyGroup)
                .ToListAsync(cancellationToken);

        public async Task<Course> AddAsync(Course entity, CancellationToken cancellationToken = default)
        {
            await _context.Courses.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(Course entity, CancellationToken cancellationToken = default)
        {
            _context.Courses.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var courseEntity = await GetByIdAsync(id, cancellationToken);
            if (courseEntity != null)
            {
                _context.Courses.Remove(courseEntity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Courses.AnyAsync(x => x.Id == id, cancellationToken);

        public async Task<IEnumerable<Course>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default)
            => await _context.Courses
                .Include(c => c.Professor)
                .Include(c => c.StudyGroup)
                .Where(c => c.StudyGroupId == groupId)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Course>> GetByProfessorIdAsync(int professorId, CancellationToken cancellationToken = default)
            => await _context.Courses
                .Include(c => c.Professor)
                .Include(c => c.StudyGroup)
                .Where(c => c.ProfessorId == professorId)
                .ToListAsync(cancellationToken);
    }
}