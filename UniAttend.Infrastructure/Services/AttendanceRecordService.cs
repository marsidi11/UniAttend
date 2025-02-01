using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Enums;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;
using UniAttend.Core.Entities;

namespace UniAttend.Infrastructure.Services
{
    /// <summary>
    /// Service for handling attendance records.
    /// </summary>
    public class AttendanceRecordService : IAttendanceRecordService
    {
        private const decimal ABSENCE_THRESHOLD = 20m;
        private const int MINIMUM_SESSIONS_FOR_ALERT = 5;

        private readonly IAttendanceRecordRepository _attendanceRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupStudentRepository _groupStudentRepository;
        private readonly ICourseSessionRepository _courseSessionRepository;
        private readonly IAbsenceAlertRepository _absenceAlertRepository;
        private readonly IStudyGroupRepository _studyGroupRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttendanceRecordService"/> class.
        /// </summary>
        public AttendanceRecordService(
            IAttendanceRecordRepository attendanceRepository,
            IStudentRepository studentRepository,
            IGroupStudentRepository groupStudentRepository,
            ICourseSessionRepository courseSessionRepository,
            IStudyGroupRepository studyGroupRepository,
            IAbsenceAlertRepository absenceAlertRepository,
            IEmailService emailService,
            IUnitOfWork unitOfWork)
        {
            _attendanceRepository = attendanceRepository;
            _studentRepository = studentRepository;
            _groupStudentRepository = groupStudentRepository;
            _courseSessionRepository = courseSessionRepository;
            _studyGroupRepository = studyGroupRepository;
            _absenceAlertRepository = absenceAlertRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Records attendance via card scan.
        /// </summary>
        /// <param name="cardId">Student card ID.</param>
        /// <param name="readerDeviceId">Reader device ID.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The created attendance record.</returns>
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

        /// <summary>
        /// Records attendance using OTP.
        /// </summary>
        /// <param name="otpCode">OTP code.</param>
        /// <param name="studentId">Student ID.</param>
        /// <param name="courseSessionId">Course session ID.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The created attendance record.</returns>
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
        /// Confirms attendance records for a course session.
        /// </summary>
        /// <param name="courseSessionId">Course session ID.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>True if confirmation was successful.</returns>
        public async Task<bool> ConfirmAttendanceAsync(int courseSessionId, CancellationToken cancellationToken = default)
        {
            var session = await _courseSessionRepository.GetByIdAsync(courseSessionId, cancellationToken);
            if (session?.StudyGroup == null)
                throw new NotFoundException($"Course session or study group not found for ID: {courseSessionId}");

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

            // Generate alerts for the entire study group
            await GenerateAlertsForStudyGroup(session.StudyGroupId, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Checks if a student can record attendance for a course session.
        /// </summary>
        /// <param name="studentId">Student ID.</param>
        /// <param name="courseSessionId">Course session ID.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>True if the student is allowed to record attendance.</returns>
        public async Task<bool> CanRecordAttendanceAsync(
            int studentId,
            int courseSessionId,
            CancellationToken cancellationToken = default)
        {
            // Check if student is enrolled in the class
            var isEnrolled = await _groupStudentRepository.IsStudentEnrolledInClassAsync(studentId, courseSessionId, cancellationToken);
            if (!isEnrolled)
                return false;

            // Check if class is active and within check-in window
            var courseSessionDetails = await _courseSessionRepository.GetByIdAsync(courseSessionId, cancellationToken);
            if (courseSessionDetails == null || !courseSessionDetails.IsActive)
                return false;

            // Check if student hasn't already checked in
            var existingRecord = await _attendanceRepository.GetStudentAttendanceForCourseSessionAsync(studentId, courseSessionId, cancellationToken);
            if (existingRecord != null)
                return false;

            return true;
        }

        /// <summary>
        /// Generates alerts for students in a study group.
        /// </summary>
        /// <param name="studyGroupId">Study group ID.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        private async Task GenerateAlertsForStudyGroup(int studyGroupId, CancellationToken cancellationToken)
        {
            var students = await _groupStudentRepository.GetStudyGroupStudentsAsync(studyGroupId, cancellationToken);
            var totalSessions = await _attendanceRepository.GetConfirmedSessionCountAsync(studyGroupId, cancellationToken);

            // Only check for alerts if minimum sessions threshold is met
            if (totalSessions < MINIMUM_SESSIONS_FOR_ALERT)
                return;

            foreach (var student in students)
            {
                var attendedSessions = await _attendanceRepository
                    .GetStudentAttendedSessionsCountAsync(student.Id, studyGroupId, cancellationToken);

                var absencePercentage = ((totalSessions - attendedSessions) / (decimal)totalSessions) * 100;

                if (absencePercentage >= ABSENCE_THRESHOLD)
                {
                    await CreateAlertIfNeeded(student.Id, studyGroupId, absencePercentage, cancellationToken);
                }
            }
        }

        /// <summary>
        /// Creates an alert if one does not already exist.
        /// </summary>
        /// <param name="studentId">Student ID.</param>
        /// <param name="studyGroupId">Study group ID.</param>
        /// <param name="absencePercentage">Calculated absence percentage.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        private async Task CreateAlertIfNeeded(int studentId, int studyGroupId, decimal absencePercentage, CancellationToken cancellationToken)
        {
            if (!await _absenceAlertRepository.HasActiveAlertAsync(studentId, studyGroupId, cancellationToken))
            {
                // Get student and study group details for email
                var student = await _studentRepository.GetByIdWithUserAsync(studentId, cancellationToken)
                    ?? throw new NotFoundException($"Student not found with ID: {studentId}");
                var studyGroup = await _studyGroupRepository.GetByIdWithDetailsAsync(studyGroupId, cancellationToken) // Change this line
                    ?? throw new NotFoundException($"Study group not found with ID: {studyGroupId}");

                // Create alert
                var alert = new AbsenceAlert(studentId, studyGroupId, absencePercentage);
                await _absenceAlertRepository.AddAsync(alert, cancellationToken);

                // Send email notification
                await _emailService.SendAbsenceAlertAsync(
                    student.User.Email,
                    $"{student.User.FirstName} {student.User.LastName}",
                    studyGroup.Subject.Name,
                    absencePercentage,
                    cancellationToken);

                // Mark alert as sent
                alert.MarkAsSent();

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Marks a student absent for a course session.
        /// </summary>
        /// <param name="courseSessionId">Course session ID.</param>
        /// <param name="studentId">Student ID.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
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