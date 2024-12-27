using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository implementation for managing GroupStudent relationships
    /// </summary>
    public class GroupStudentRepository : IGroupStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupStudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GroupStudent> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _context.GroupStudents
                .Include(gs => gs.Student)
                .Include(gs => gs.Group)
                .FirstOrDefaultAsync(gs => gs.Id == id, cancellationToken);

        public async Task<IEnumerable<GroupStudent>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _context.GroupStudents
                .Include(gs => gs.Student)
                .Include(gs => gs.Group)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<GroupStudent>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default)
            => await _context.GroupStudents
                .Where(gs => gs.GroupId == groupId)
                .Include(gs => gs.Student)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<GroupStudent>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default)
            => await _context.GroupStudents
                .Where(gs => gs.StudentId == studentId)
                .Include(gs => gs.Group)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<GroupStudent>> GetActiveGroupsByStudentIdAsync(int studentId, int academicYearId, CancellationToken cancellationToken = default)
            => await _context.GroupStudents
                .Where(gs => gs.StudentId == studentId && gs.Group.AcademicYearId == academicYearId)
                .Include(gs => gs.Group)
                .ToListAsync(cancellationToken);

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
            => await _context.GroupStudents.AnyAsync(gs => gs.Id == id, cancellationToken);

        public async Task<bool> ExistsAsync(int groupId, int studentId, CancellationToken cancellationToken = default)
            => await _context.GroupStudents
                .AnyAsync(gs => gs.GroupId == groupId && gs.StudentId == studentId, cancellationToken);

        public async Task<GroupStudent> AddAsync(GroupStudent entity, CancellationToken cancellationToken = default)
        {
            await _context.GroupStudents.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task AddStudentToGroupAsync(int groupId, int studentId, CancellationToken cancellationToken = default)
        {
            var groupStudent = new GroupStudent(groupId, studentId);
            await _context.GroupStudents.AddAsync(groupStudent, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(GroupStudent entity, CancellationToken cancellationToken = default)
        {
            _context.GroupStudents.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var groupStudent = await GetByIdAsync(id, cancellationToken);
            if (groupStudent != null)
            {
                _context.GroupStudents.Remove(groupStudent);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task RemoveStudentFromGroupAsync(int groupId, int studentId, CancellationToken cancellationToken = default)
        {
            var groupStudent = await _context.GroupStudents
                .FirstOrDefaultAsync(gs => gs.GroupId == groupId && gs.StudentId == studentId, cancellationToken);
            if (groupStudent != null)
            {
                _context.GroupStudents.Remove(groupStudent);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}