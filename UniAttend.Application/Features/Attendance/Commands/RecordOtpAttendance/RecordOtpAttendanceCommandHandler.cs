using MediatR;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Application.Features.Attendance.DTOs;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;
using UniAttend.Core.Enums;
using UniAttend.Core.Entities;

namespace UniAttend.Application.Features.Attendance.Commands.RecordOtpAttendance
{
    public class RecordOtpAttendanceCommandHandler : IRequestHandler<RecordOtpAttendanceCommand, AttendanceRecordDto>
    {
        private readonly IAttendanceService _attendanceService;
        private readonly ITotpService _totpService;
        private readonly IStudentRepository _studentRepository;
        private readonly IRateLimiter _rateLimiter;

        public RecordOtpAttendanceCommandHandler(
            IAttendanceService attendanceService,
            ITotpService totpService,
            IStudentRepository studentRepository,
            IRateLimiter rateLimiter)
        {
            _attendanceService = attendanceService;
            _totpService = totpService;
            _studentRepository = studentRepository;
            _rateLimiter = rateLimiter;
        }

        public async Task<AttendanceRecordDto> Handle(RecordOtpAttendanceCommand request, CancellationToken cancellationToken)
        {
            if (!await _rateLimiter.CheckAsync($"totp:{request.StudentId}", 3, TimeSpan.FromMinutes(5)))
                throw new TooManyRequestsException("Too many TOTP verification attempts. Please try again later.");

            var student = await _studentRepository.GetByIdWithUserAsync(request.StudentId, cancellationToken)
                ?? throw new NotFoundException("Student not found");

            if (string.IsNullOrEmpty(student.User.TotpSecret))
                throw new ValidationException("TOTP not set up for this user");

            if (request.VerificationType == VerificationType.Totp)
            {
                if (!_totpService.VerifyTotp(student.User.TotpSecret, request.OtpCode))
                    throw new ValidationException("Invalid TOTP code");
            }

            var record = await _attendanceService.RecordOtpAttendanceAsync(
                request.OtpCode,
                request.StudentId,
                request.CourseSessionId,
                cancellationToken);

            return new AttendanceRecordDto(
                record.Id,
                record.CourseSessionId,
                record.StudentId,
                $"{record.Student?.User?.FirstName} {record.Student?.User?.LastName}",
                record.CheckInTime,
                record.CheckInMethod,
                record.IsConfirmed,
                record.IsAbsent,
                record.ConfirmationTime,
                record.CourseSession.StudyGroup.Name,
                record.CourseSession.Classroom.Name,
                record.CourseSession.StartTime,
                record.CourseSession.EndTime
            );
        }
    }
}