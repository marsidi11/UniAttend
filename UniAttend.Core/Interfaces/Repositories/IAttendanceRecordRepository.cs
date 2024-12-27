using UniAttend.Core.Entities.Attendance;
using System.Collections.Generic;

namespace UniAttend.Core.Interfaces.Repositories
{
    /// <summary>
    /// Interface for managing attendance records.
    /// </summary>
    public interface IAttendanceRecordRepository : IRepository<AttendanceRecord>
    {
        Task<IEnumerable<AttendanceRecord>> GetByClassIdAsync(int classId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AttendanceRecord>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AttendanceRecord>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<AttendanceRecord?> GetStudentAttendanceForClassAsync(int studentId, int classId, CancellationToken cancellationToken = default);
        Task<double> GetStudentAttendancePercentageAsync(int studentId, int groupId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AttendanceRecord>> GetUnconfirmedRecordsAsync(int classId, CancellationToken cancellationToken = default);
        Task ConfirmAttendanceRecordsAsync(int classId, CancellationToken cancellationToken = default);
    }
}