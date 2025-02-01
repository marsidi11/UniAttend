using UniAttend.Core.Entities.Attendance;

namespace UniAttend.Core.Interfaces.Services
{
    /// <summary>
    /// Provides attendance management operations for the university attendance system
    /// </summary>
    public interface IAttendanceRecordService
    {
        /// <summary>
        /// Records attendance using a student's ID card
        /// </summary>
        Task<AttendanceRecord> RecordCardAttendanceAsync(string cardId, string readerDeviceId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Records attendance using a one-time password
        /// </summary>
        Task<AttendanceRecord> RecordOtpAttendanceAsync(string otpCode, int studentId, int courseSessionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Confirms attendance records for a specific course session
        /// </summary>
        Task<bool> ConfirmAttendanceAsync(int courseSessionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates if a student can record attendance for a specific class
        /// </summary>
        Task<bool> CanRecordAttendanceAsync(int studentId, int courseSessionId, CancellationToken cancellationToken = default);

        // <summary>
        /// Marks a student as absent for a specific class
        /// </summary>
        Task MarkStudentAbsentAsync(int courseSessionId, int studentId, CancellationToken cancellationToken = default);
    }
}