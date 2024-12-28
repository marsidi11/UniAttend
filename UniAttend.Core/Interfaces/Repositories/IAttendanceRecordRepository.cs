using UniAttend.Core.Entities.Attendance;
using System.Collections.Generic;

namespace UniAttend.Core.Interfaces.Repositories
{
    /// <summary>
    /// Interface for managing attendance records.
    /// </summary>
    public interface IAttendanceRecordRepository : IRepository<AttendanceRecord>
    {
        Task<IEnumerable<AttendanceRecord>> GetByCourseIdAsync(int courseId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AttendanceRecord>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AttendanceRecord>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<AttendanceRecord?> GetStudentAttendanceForCourseAsync(int studentId, int courseId, CancellationToken cancellationToken = default);
        Task<double> GetStudentAttendancePercentageAsync(int studentId, int groupId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AttendanceRecord>> GetUnconfirmedRecordsAsync(int courseId, CancellationToken cancellationToken = default);
        Task ConfirmAttendanceRecordsAsync(int courseId, CancellationToken cancellationToken = default);
    }
}