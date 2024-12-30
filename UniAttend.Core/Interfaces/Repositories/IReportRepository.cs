using UniAttend.Core.Entities;
using UniAttend.Core.Entities.Attendance;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UniAttend.Core.Interfaces.Repositories
{
    public interface IReportRepository
    {
        Task<IEnumerable<AttendanceRecord>> GetAttendanceRecordsAsync(
            DateTime startDate,
            DateTime endDate,
            int? departmentId = null,
            int? subjectId = null, 
            int? groupId = null,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Student>> GetStudentsWithHighAbsenceAsync(
            double absenceThreshold,
            CancellationToken cancellationToken = default);
    }
}