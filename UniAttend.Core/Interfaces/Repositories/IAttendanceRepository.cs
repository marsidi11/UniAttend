using UniAttend.Core.Entities.Attendance;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<AttendanceRecord>> GetAttendanceByDateRangeAsync(
            DateTime startDate, 
            DateTime endDate, 
            int? departmentId = null,
            int? subjectId = null,
            int? groupId = null,
            CancellationToken cancellationToken = default);
            
        Task<IEnumerable<AttendanceRecord>> GetAbsencesByThresholdAsync(
            double absenceThreshold,
            CancellationToken cancellationToken = default);
    }
}