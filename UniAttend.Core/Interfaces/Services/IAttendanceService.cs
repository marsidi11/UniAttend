using UniAttend.Core.Entities.Attendance;

namespace UniAttend.Core.Interfaces.Services
{
    /// <summary>
    /// Provides attendance management operations for the university attendance system
    /// </summary>
    public interface IAttendanceService
    {
        /// <summary>
        /// Records attendance using a student's ID card
        /// </summary>
        Task<AttendanceRecord> RecordCardAttendanceAsync(string cardId, string readerDeviceId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Records attendance using a one-time password
        /// </summary>
        Task<AttendanceRecord> RecordOtpAttendanceAsync(string otpCode, int studentId, int classId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Confirms attendance records for a specific class by professor
        /// </summary>
        Task<bool> ConfirmAttendanceAsync(int classId, int professorId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates alerts for students with high absence rates
        /// </summary>
        Task GenerateAbsenceAlertsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates if a student can record attendance for a specific class
        /// </summary>
        Task<bool> CanRecordAttendanceAsync(int studentId, int classId, CancellationToken cancellationToken = default);
    }
}