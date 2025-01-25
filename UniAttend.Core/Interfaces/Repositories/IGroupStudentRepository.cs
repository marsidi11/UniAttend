using UniAttend.Core.Entities;

namespace UniAttend.Core.Interfaces.Repositories 
{
    public interface IGroupStudentRepository : IRepository<GroupStudent>
    {
        Task<IEnumerable<GroupStudent>> GetByGroupIdAsync(int studyGroupId, CancellationToken cancellationToken = default);

        Task<IEnumerable<GroupStudent>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(int studyGroupId, int studentId, CancellationToken cancellationToken = default);

        Task AddStudentToGroupAsync(int studyGroupId, int studentId, CancellationToken cancellationToken = default);

        Task RemoveStudentFromStudyGroupAsync(int studyGroupId, int studentId, CancellationToken cancellationToken = default);

        Task<IEnumerable<GroupStudent>> GetActiveGroupsByStudentIdAsync(int studentId, int academicYearId, CancellationToken cancellationToken = default);

        Task<IEnumerable<GroupStudent>> GetByGroupIdWithDetailsAsync(int studyGroupId, CancellationToken cancellationToken = default);

        Task<bool> IsStudentEnrolledInClassAsync(int studentId, int courseSessionId, CancellationToken cancellationToken = default);

        Task<IEnumerable<GroupStudent>> GetAllActiveGroupsAsync(CancellationToken cancellationToken = default);
        
        Task<IEnumerable<GroupStudent>> GetStudyGroupStudentsAsync(int groupId, CancellationToken cancellationToken = default);
    }
}