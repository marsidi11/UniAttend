using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Enums;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Infrastructure.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRecordRepository _attendanceRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupStudentRepository _groupStudentRepository;
        private readonly ICourseSessionRepository _courseSessionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceService(
            IAttendanceRecordRepository attendanceRepository,
            IStudentRepository studentRepository,
            IGroupStudentRepository groupStudentRepository,
            ICourseSessionRepository courseSessionRepository,
            IUnitOfWork unitOfWork)
        {
            _attendanceRepository = attendanceRepository;
            _studentRepository = studentRepository;
            _groupStudentRepository = groupStudentRepository;
            _courseSessionRepository = courseSessionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AttendanceRecord> RecordCardAttendanceAsync(
            string cardId,
            string readerDeviceId,
            CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetByCardIdAsync(cardId, cancellationToken)
                ?? throw new NotFoundException($"No student found with card ID: {cardId}");

            var activeCourseSession = await _courseSessionRepository.GetActiveByDeviceIdAsync(readerDeviceId, cancellationToken)
                ?? throw new ValidationException($"No active course session found for reader device: {readerDeviceId}");

            if (!await CanRecordAttendanceAsync(student.Id, activeCourseSession.Id, cancellationToken))
                throw new ValidationException("Student cannot record attendance for this course session");

            var record = new AttendanceRecord(
                activeCourseSession.Id,
                student.Id,
                DateTime.UtcNow,
                CheckInMethod.Card
            );

            await _attendanceRepository.AddAsync(record, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return record;
        }

        public async Task<AttendanceRecord> RecordOtpAttendanceAsync(
            string otpCode,
            int studentId,
            int courseSessionId,
            CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetByIdAsync(studentId, cancellationToken)
                ?? throw new NotFoundException($"Student not found with ID: {studentId}");

            if (!await CanRecordAttendanceAsync(studentId, courseSessionId, cancellationToken))
                throw new ValidationException("Student cannot record attendance for this course session");

            var record = new AttendanceRecord(
                courseSessionId,
                studentId,
                DateTime.UtcNow,
                CheckInMethod.Totp
            );

            await _attendanceRepository.AddAsync(record, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return record;
        }

        /// <summary>
        /// Confirms attendance records for a course session
        /// </summary>
        /// <param name="courseSessionId">ID of the course session</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if confirmation was successful</returns>
                public async Task<bool> ConfirmAttendanceAsync(int courseSessionId, CancellationToken cancellationToken = default)
        {
            var session = await _courseSessionRepository.GetByIdAsync(courseSessionId, cancellationToken);
            
            // Get all enrolled students
            var enrolledStudents = session.StudyGroup.Students;
            
            // Get existing attendance records
            var existingRecords = await _attendanceRepository.GetByCourseSessionIdAsync(courseSessionId, cancellationToken);
            
            // Create absent records for students without attendance
            foreach (var enrollment in enrolledStudents)
            {
                if (!existingRecords.Any(r => r.StudentId == enrollment.StudentId))
                {
                    var absentRecord = new AttendanceRecord(
                        courseSessionId,
                        enrollment.StudentId,
                        session.Date,
                        CheckInMethod.Manual
                    );
                    absentRecord.MarkAsAbsent();
                    await _attendanceRepository.AddAsync(absentRecord, cancellationToken);
                }
            }
        
            // Confirm existing records
            await _attendanceRepository.ConfirmAttendanceRecordsAsync(courseSessionId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            return true;
        }

        public async Task<bool> CanRecordAttendanceAsync(
            int studentId,
            int courseSessionId,
            CancellationToken cancellationToken = default)
        {
            // Check if student is enrolled in the class
            var isEnrolled = await _groupStudentRepository.IsStudentEnrolledInClassAsync(studentId, courseSessionId, cancellationToken);
            if (!isEnrolled) return false;

            // Check if class is active and within check-in window
            var courseSessionDetails = await _courseSessionRepository.GetByIdAsync(courseSessionId, cancellationToken);
            if (courseSessionDetails == null || !courseSessionDetails.IsActive) return false;

            // Check if student hasn't already checked in
            var existingRecord = await _attendanceRepository.GetStudentAttendanceForCourseSessionAsync(studentId, courseSessionId, cancellationToken);
            if (existingRecord != null) return false;

            return true;
        }

        public async Task GenerateAbsenceAlertsAsync(CancellationToken cancellationToken = default)
        {
            var studyGroups = await _groupStudentRepository.GetAllActiveGroupsAsync(cancellationToken);

            foreach (var studyGroup in studyGroups)
            {
                var students = await _groupStudentRepository.GetStudyGroupStudentsAsync(studyGroup.Id, cancellationToken);

                foreach (var student in students)
                {
                    var attendanceRate = await _attendanceRepository
                        .GetStudentAttendancePercentageAsync(student.Id, studyGroup.Id, cancellationToken);

                    if (attendanceRate < 75) // Configurable threshold
                    {
                        // TODO: Generate and send alert through notification service
                        // await _notificationService.SendAbsenceAlertAsync(student.Id, attendanceRate);
                    }
                }
            }
        }

        public async Task MarkStudentAbsentAsync(
            int courseSessionId, 
            int studentId,
            CancellationToken cancellationToken = default)
        {
            var existingRecord = await _attendanceRepository
                .GetStudentAttendanceForCourseSessionAsync(studentId, courseSessionId, cancellationToken);
                
            if (existingRecord != null)
            {
                existingRecord.MarkAsAbsent(); 
            }
            else
            {
                var newRecord = new AttendanceRecord(
                    courseSessionId,
                    studentId,
                    DateTime.UtcNow,
                    CheckInMethod.Manual
                );
                newRecord.MarkAsAbsent();
                await _attendanceRepository.AddAsync(newRecord, cancellationToken);
            }
        
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}