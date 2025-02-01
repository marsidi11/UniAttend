using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Entities.Stats;

namespace UniAttend.Core.Interfaces.Repositories
{
    /// <summary>
    /// Interface for managing attendance records.
    /// </summary>
    public interface IAttendanceRecordRepository : IRepository<AttendanceRecord>
    {
        /// <summary>
        /// Retrieves attendance records by course session ID.
        /// </summary>
        Task<IEnumerable<AttendanceRecord>> GetByCourseSessionIdAsync(
            int courseSessionId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves attendance records by student ID.
        /// </summary>
        Task<IEnumerable<AttendanceRecord>> GetByStudentIdAsync(
            int studentId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a student's attendance record for a specific session.
        /// </summary>
        Task<AttendanceRecord?> GetStudentAttendanceForSessionAsync(
            int studentId, 
            int courseSessionId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves attendance records within a date range.
        /// </summary>
        Task<IEnumerable<AttendanceRecord>> GetByDateRangeAsync(
            DateTime startDate, 
            DateTime endDate, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a student's attendance record for a course.
        /// </summary>
        Task<AttendanceRecord?> GetStudentAttendanceForCourseSessionAsync(
            int studentId, 
            int courseId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calculates a student's attendance percentage for a study group.
        /// </summary>
        Task<double> GetStudentAttendancePercentageAsync(
            int studentId, 
            int studyGroupId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves unconfirmed attendance records for a course.
        /// </summary>
        Task<IEnumerable<AttendanceRecord>> GetUnconfirmedRecordsAsync(
            int courseId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts confirmed course sessions for a study group.
        /// </summary>
        Task<int> GetConfirmedSessionCountAsync(
            int studyGroupId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts sessions attended by a student in a study group.
        /// </summary>
        Task<int> GetStudentAttendedSessionsCountAsync(
            int studentId, 
            int studyGroupId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Confirms attendance records for a course session.
        /// </summary>
        Task ConfirmAttendanceRecordsAsync(
            int courseSessionId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a course session with additional details.
        /// </summary>
        Task<CourseSession> GetSessionWithDetailsAsync(
            int sessionId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves total and attended course sessions for a student in a study group.
        /// </summary>
        Task<(int TotalCourseSessions, int AttendedCourseSessions)> GetStudentGroupAttendanceAsync(
            int studentId,
            int studyGroupId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates an attendance report for an academic year.
        /// </summary>
        Task<AttendanceReportResult> GetAcademicYearAttendanceReportAsync(
            int academicYearId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates an attendance report for a study group.
        /// </summary>
        Task<AttendanceReportResult> GetGroupAttendanceReportAsync(
            int studyGroupId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves attendance statistics for a student.
        /// </summary>
        Task<AttendanceStats> GetStudentStatsAsync(
            int studentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves detailed attendance records by course session ID.
        /// </summary>
        Task<IEnumerable<AttendanceRecord>> GetDetailedByCourseSessionIdAsync(
            int courseSessionId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves detailed attendance records for a student.
        /// </summary>
        Task<IEnumerable<AttendanceRecord>> GetDetailedStudentAttendanceAsync(
            int studentId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves attendance records for a study group within a date range.
        /// </summary>
        Task<IEnumerable<AttendanceRecord>> GetGroupAttendanceAsync(
            int studyGroupId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves attendance statistics for a department.
        /// </summary>
        Task<AttendanceStats> GetDepartmentAttendanceStatsAsync(
            int departmentId,
            int? academicYearId,
            CancellationToken cancellationToken = default);
    }
}