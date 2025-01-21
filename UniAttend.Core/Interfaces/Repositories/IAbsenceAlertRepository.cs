using UniAttend.Core.Entities;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IAbsenceAlertRepository : IRepository<AbsenceAlert>
    {
        Task<IEnumerable<AbsenceAlert>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AbsenceAlert>> GetByGroupIdAsync(int studyGroupId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AbsenceAlert>> GetUnsentAlertsAsync(CancellationToken cancellationToken = default);
        Task MarkAlertAsSentAsync(int alertId, CancellationToken cancellationToken = default);
        Task<bool> HasActiveAlertAsync(int studentId, int studyGroupId, CancellationToken cancellationToken = default);
    }
}