using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Entities.Stats;
using System.Collections.Generic;

namespace UniAttend.Core.Interfaces.Repositories
{
    /// <summary>
    /// Interface for managing attendance records.
    /// </summary>
    public interface IAttendanceRecordRepository : IRepository<AttendanceRecord>
    {
        Task<IEnumerable<AttendanceRecord>> GetByCourseSessionIdAsync(int courseSessionId, CancellationToken cancellationToken = default);

        Task<IEnumerable<AttendanceRecord>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default);

        Task<AttendanceRecord?> GetStudentAttendanceForSessionAsync(int studentId, int courseSessionId, CancellationToken cancellationToken = default);

        Task<IEnumerable<AttendanceRecord>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

        Task<AttendanceRecord?> GetStudentAttendanceForCourseSessionAsync(int studentId, int courseId, CancellationToken cancellationToken = default);

        Task<double> GetStudentAttendancePercentageAsync(int studentId, int studyGroupId, CancellationToken cancellationToken = default);

        Task<IEnumerable<AttendanceRecord>> GetUnconfirmedRecordsAsync(int courseId, CancellationToken cancellationToken = default);

        Task ConfirmAttendanceRecordsAsync(int courseSessionId, CancellationToken cancellationToken = default);

        Task<CourseSession> GetSessionWithDetailsAsync(
            int sessionId,
            CancellationToken cancellationToken = default);

        Task<(int TotalCourseSessions, int AttendedCourseSessions)> GetStudentGroupAttendanceAsync(
        int studentId,
        int studyGroupId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);

        Task<AttendanceReportResult> GetAcademicYearAttendanceReportAsync(
        int academicYearId,
        CancellationToken cancellationToken = default);

        Task<AttendanceReportResult> GetGroupAttendanceReportAsync(
            int studyGroupId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default);

        Task<AttendanceStats> GetStudentStatsAsync(int studentId, CancellationToken cancellationToken = default);

        Task<IEnumerable<AttendanceRecord>> GetDetailedByCourseSessionIdAsync(
        int courseSessionId,
        CancellationToken cancellationToken = default);

        Task<IEnumerable<AttendanceRecord>> GetDetailedStudentAttendanceAsync(
            int studentId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<AttendanceRecord>> GetGroupAttendanceAsync(
        int studyGroupId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets attendance statistics for a department
        /// </summary>
        /// <param name="departmentId">ID of the department</param>
        /// <param name="academicYearId">Optional academic year filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Department attendance statistics</returns>
        Task<AttendanceStats> GetDepartmentAttendanceStatsAsync(
            int departmentId,
            int? academicYearId,
            CancellationToken cancellationToken = default);
    }
}