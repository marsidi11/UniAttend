using UniAttend.Core.Entities.Attendance;
using System.Collections.Generic;

namespace UniAttend.Core.Interfaces.Repositories
{
    /// <summary>
    /// Interface for managing attendance records.
    /// </summary>
    public interface IAttendanceRecordRepository
    {
        void Add(AttendanceRecord attendanceRecord);
        List<AttendanceRecord> GetByClassId(int classId);
        AttendanceRecord? GetById(int id);
        void Update(AttendanceRecord attendanceRecord);
        void Delete(int id);
    }
}