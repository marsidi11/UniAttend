using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Stats;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IStudyGroupRepository : IRepository<StudyGroup>
{
    Task<StudyGroup?> GetWithStudentsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<StudyGroup>> GetByProfessorIdAsync(int professorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<StudyGroup>> GetBySubjectIdAsync(int subjectId, CancellationToken cancellationToken = default);
    Task<bool> HasStudentAsync(int groupId, int studentId, CancellationToken cancellationToken = default);
    Task<StudyGroup?> GetWithScheduleAsync(int id, CancellationToken cancellationToken = default);
    Task<AttendanceStats> GetAttendanceStatsAsync(int groupId, CancellationToken cancellationToken = default);
}
}