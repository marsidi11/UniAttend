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
                .Include(gs => gs.StudyGroup)
                .FirstOrDefaultAsync(gs => gs.Id == id, cancellationToken);

        public async Task<bool> IsStudentEnrolledInClassAsync(
            int studentId,
            int courseSessionId,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AnyAsync(gs =>
                    gs.StudentId == studentId &&
                    gs.StudyGroup.CourseSessions.Any(cs => cs.Id == courseSessionId),
                    cancellationToken);
        }

        public async Task<IEnumerable<GroupStudent>> GetAllActiveGroupsAsync(
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(gs => gs.StudyGroup)
                .Where(gs => gs.StudyGroup.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<GroupStudent>> GetStudyGroupStudentsAsync(
            int groupId,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(gs => gs.Student)
                    .ThenInclude(s => s.User)
                .Where(gs => gs.StudyGroupId == groupId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<GroupStudent>> GetByGroupIdAsync(int studyGroupId, CancellationToken cancellationToken = default)
            => await DbSet
                .Where(gs => gs.StudyGroupId == studyGroupId)
                .Include(gs => gs.Student)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<GroupStudent>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default)
            => await DbSet
                .Where(gs => gs.StudentId == studentId)
                .Include(gs => gs.StudyGroup)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<GroupStudent>> GetActiveGroupsByStudentIdAsync(int studentId, int academicYearId, CancellationToken cancellationToken = default)
            => await DbSet
                .Where(gs => gs.StudentId == studentId && gs.StudyGroup.AcademicYearId == academicYearId)
                .Include(gs => gs.StudyGroup)
                .ToListAsync(cancellationToken);

        public async Task<bool> ExistsAsync(int studyGroupId, int studentId, CancellationToken cancellationToken = default)
            => await DbSet.AnyAsync(gs => gs.StudyGroupId == studyGroupId && gs.StudentId == studentId, cancellationToken);

        public async Task AddStudentToGroupAsync(int studyGroupId, int studentId, CancellationToken cancellationToken = default)
        {
            var groupStudent = new GroupStudent(studyGroupId, studentId);
            await AddAsync(groupStudent, cancellationToken);
        }

        public async Task RemoveStudentFromStudyGroupAsync(int studyGroupId, int studentId, CancellationToken cancellationToken = default)
        {
            var groupStudent = await DbSet
                .FirstOrDefaultAsync(gs => gs.StudyGroupId == studyGroupId && gs.StudentId == studentId, cancellationToken);

            if (groupStudent != null)
            {
                await DeleteAsync(groupStudent.Id, cancellationToken);
            }
        }

        public async Task<IEnumerable<GroupStudent>> GetByGroupIdWithDetailsAsync(int studyGroupId, CancellationToken cancellationToken)
        {
            return await DbSet
                .Include(gs => gs.Student)
                    .ThenInclude(s => s.User)
                .Where(gs => gs.StudyGroupId == studyGroupId)
                .ToListAsync(cancellationToken);
        }
    }
}