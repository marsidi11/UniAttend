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
        Task<IEnumerable<AttendanceRecord>> GetGroupAttendanceAsync(
            int groupId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default);

        Task<CourseSession> GetSessionWithDetailsAsync(
            int sessionId,
            CancellationToken cancellationToken = default);

        Task<(int TotalClasses, int AttendedClasses)> GetStudentGroupAttendanceAsync(
        int studentId,
        int groupId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);

        Task<AttendanceReportResult> GetAcademicYearAttendanceReportAsync(
        int academicYearId,
        CancellationToken cancellationToken = default);
        
    Task<AttendanceReportResult> GetGroupAttendanceReportAsync(
        int groupId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);   
    }
}