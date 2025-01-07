using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Stats;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Infrastructure.Data.Repositories
{
    public class StudyGroupRepository : BaseRepository<StudyGroup>, IStudyGroupRepository
    {
        public StudyGroupRepository(ApplicationDbContext context) : base(context) { }

        public async Task<StudyGroup?> GetWithStudentsAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(g => g.Students)
                    .ThenInclude(gs => gs.Student)
                        .ThenInclude(s => s!.User)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        public async Task<IEnumerable<StudyGroup>> GetByProfessorIdAsync(int professorId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(g => g.Subject)
                .Include(g => g.Professor)
                .Where(g => g.ProfessorId == professorId)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<StudyGroup>> GetBySubjectIdAsync(int subjectId, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(g => g.Subject)
                .Include(g => g.Professor)
                .Where(g => g.SubjectId == subjectId)
                .ToListAsync(cancellationToken);

        public async Task<bool> HasStudentAsync(int groupId, int studentId, CancellationToken cancellationToken = default)
            => await DbSet
                .AnyAsync(g => g.Id == groupId &&
                    g.Students.Any(s => s.StudentId == studentId),
                    cancellationToken);

        public async Task<StudyGroup?> GetWithScheduleAsync(int id, CancellationToken cancellationToken = default)
            => await DbSet
                .Include(g => g.Schedules)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        public async Task<AttendanceStats> GetAttendanceStatsAsync(int groupId, CancellationToken cancellationToken = default)
        {
            var group = await DbSet
                .Include(g => g.Students)
                .Include(g => g.AttendanceRecords)
                .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);

            if (group == null)
                throw new NotFoundException($"Group with ID {groupId} not found");

            return new AttendanceStats
            {
                TotalStudents = group.Students.Count,
                AverageAttendance = group.AttendanceRecords.Count > 0
                    ? (decimal)group.AttendanceRecords.Average(r => r.IsConfirmed ? 1 : 0) * 100m
                    : 0m
            };
        }
    }
}