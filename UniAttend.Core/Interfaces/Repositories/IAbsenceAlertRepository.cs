using UniAttend.Core.Entities;

namespace UniAttend.Core.Interfaces.Repositories
{
    /// <summary>
    /// Interface for managing AbsenceAlert entities.
    /// </summary>
    public interface IAbsenceAlertRepository : IRepository<AbsenceAlert>
    {
        /// <summary>
        /// Retrieves absence alerts for a given student.
        /// </summary>
        Task<IEnumerable<AbsenceAlert>> GetByStudentIdAsync(
            int studentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves absence alerts for a given study group.
        /// </summary>
        Task<IEnumerable<AbsenceAlert>> GetByGroupIdAsync(
            int studyGroupId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all absence alerts that have not been sent.
        /// </summary>
        Task<IEnumerable<AbsenceAlert>> GetUnsentAlertsAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks the specified absence alert as sent.
        /// </summary>
        Task MarkAlertAsSentAsync(
            int alertId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an active unsent alert exists for a student in a study group.
        /// </summary>
        Task<bool> HasActiveAlertAsync(
            int studentId,
            int studyGroupId,
            CancellationToken cancellationToken = default);
    }
}