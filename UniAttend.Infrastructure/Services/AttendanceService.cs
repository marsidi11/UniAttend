using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Infrastructure.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRecordRepository _attendanceRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupStudentRepository _groupStudentRepository;

        public AttendanceService(
            IAttendanceRecordRepository attendanceRepository,
            IStudentRepository studentRepository,
            IGroupStudentRepository groupStudentRepository)
        {
            _attendanceRepository = attendanceRepository;
            _studentRepository = studentRepository;
            _groupStudentRepository = groupStudentRepository;
        }

        public Task<AttendanceRecord> RecordCardAttendanceAsync(
            string cardId,
            string readerDeviceId,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AttendanceRecord> RecordOtpAttendanceAsync(
            string otpCode,
            int studentId,
            int classId,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ConfirmAttendanceAsync(
            int classId,
            int professorId,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task GenerateAbsenceAlertsAsync(
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CanRecordAttendanceAsync(
            int studentId,
            int classId,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}