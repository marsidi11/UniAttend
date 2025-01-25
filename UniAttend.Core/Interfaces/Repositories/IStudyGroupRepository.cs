using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Stats;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IStudyGroupRepository : IRepository<StudyGroup>
    {
        Task<StudyGroup?> GetWithStudentsAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<StudyGroup>> GetByProfessorIdAsync(int professorId, CancellationToken cancellationToken = default);
        Task<IEnumerable<StudyGroup>> GetBySubjectIdAsync(int subjectId, CancellationToken cancellationToken = default);
        Task<bool> HasStudentAsync(int studyGroupId, int studentId, CancellationToken cancellationToken = default);
        Task<StudyGroup?> GetWithScheduleAsync(int id, CancellationToken cancellationToken = default);
        Task<AttendanceStats> GetAttendanceStatsAsync(int studyGroupId, CancellationToken cancellationToken = default);
        Task<IEnumerable<StudyGroup>> GetByDepartmentIdAsync(
            int departmentId,
            int? academicYearId = null,
            CancellationToken cancellationToken = default);
        Task<StudyGroup?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsWithNameAsync(string name, int subjectId, int academicYearId, CancellationToken cancellationToken = default);
        Task<IEnumerable<StudyGroup>> GetByProfessorIdAsync(int professorId, int? academicYearId = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<StudyGroup>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<StudyGroup>> GetStudentGroupsAsync(int studentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<StudyGroup>> GetProfessorStudyGroupsAsync(int professorId, CancellationToken cancellationToken = default);
        Task<IEnumerable<StudyGroup>> GetAllWithDetailsAsync(int? academicYearId = null, CancellationToken cancellationToken = default);
    }
}