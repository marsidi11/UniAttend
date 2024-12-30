using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository implementation for managing GroupStudent relationships
    /// </summary>
    public class GroupStudentRepository : BaseRepository<GroupStudent>, IGroupStudentRepository
    {
        public GroupStudentRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<GroupStudent?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(gs => gs.Student)
                .Include(gs => gs.Group)
                .FirstOrDefaultAsync(gs => gs.Id == id, cancellationToken);

        public async Task<IEnumerable<GroupStudent>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default)
            => await DbSet
                .Where(gs => gs.GroupId == groupId)
                .Include(gs => gs.Student)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<GroupStudent>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default)
            => await DbSet
                .Where(gs => gs.StudentId == studentId)
                .Include(gs => gs.Group)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<GroupStudent>> GetActiveGroupsByStudentIdAsync(int studentId, int academicYearId, CancellationToken cancellationToken = default)
            => await DbSet
                .Where(gs => gs.StudentId == studentId && gs.Group.AcademicYearId == academicYearId)
                .Include(gs => gs.Group)
                .ToListAsync(cancellationToken);

        public async Task<bool> ExistsAsync(int groupId, int studentId, CancellationToken cancellationToken = default)
            => await DbSet.AnyAsync(gs => gs.GroupId == groupId && gs.StudentId == studentId, cancellationToken);

        public async Task AddStudentToGroupAsync(int groupId, int studentId, CancellationToken cancellationToken = default)
        {
            var groupStudent = new GroupStudent(groupId, studentId);
            await AddAsync(groupStudent, cancellationToken);
        }

        public async Task RemoveStudentFromGroupAsync(int groupId, int studentId, CancellationToken cancellationToken = default)
        {
            var groupStudent = await DbSet
                .FirstOrDefaultAsync(gs => gs.GroupId == groupId && gs.StudentId == studentId, cancellationToken);
            
            if (groupStudent != null)
            {
                await DeleteAsync(groupStudent.Id, cancellationToken);
            }
        }
    }
}