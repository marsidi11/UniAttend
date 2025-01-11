using MediatR;
using Microsoft.Extensions.Logging;
using UniAttend.Application.Features.Reports.DTOs;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;


namespace UniAttend.Application.Features.Reports.Queries.GetStudentReport
{
    public class GetStudentReportQueryHandler : IRequestHandler<GetStudentReportQuery, StudentReportDto>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IAttendanceRecordRepository _attendanceRepository;
        private readonly IGroupStudentRepository _groupStudentRepository;
        private readonly ILogger<GetStudentReportQueryHandler> _logger;

        public GetStudentReportQueryHandler(
            IStudentRepository studentRepository,
            IAttendanceRecordRepository attendanceRepository, 
            IGroupStudentRepository groupStudentRepository,
            ILogger<GetStudentReportQueryHandler> logger)
        {
            _studentRepository = studentRepository;
            _attendanceRepository = attendanceRepository;
            _groupStudentRepository = groupStudentRepository;
            _logger = logger;
        }

        public async Task<StudentReportDto> Handle(GetStudentReportQuery request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken) 
                ?? throw new NotFoundException($"Student with ID {request.StudentId} not found");

            var enrollments = await _groupStudentRepository.GetByStudentIdAsync(request.StudentId, cancellationToken);
            var subjects = new List<SubjectAttendanceDto>();
            int totalClasses = 0;
            int totalAttendance = 0;

            foreach (var enrollment in enrollments)
            {
                var attendance = await _attendanceRepository
                    .GetStudentGroupAttendanceAsync(
                        request.StudentId, 
                        enrollment.GroupId,
                        request.StartDate,
                        request.EndDate, 
                        cancellationToken);

                totalClasses += attendance.TotalClasses;
                totalAttendance += attendance.AttendedClasses;

                subjects.Add(new SubjectAttendanceDto
                {
                    SubjectId = enrollment.Group?.SubjectId ?? 0,
                    SubjectName = enrollment.Group?.Subject?.Name ?? "Unknown",
                    GroupName = enrollment.Group?.Name ?? "Unknown",
                    AttendedClasses = attendance.AttendedClasses,
                    TotalClasses = attendance.TotalClasses,
                    AttendanceRate = attendance.TotalClasses > 0 
                        ? (decimal)attendance.AttendedClasses / attendance.TotalClasses * 100 
                        : 0
                });
            }

            return new StudentReportDto
            {
                StudentId = request.StudentId,
                StudentNumber = student.StudentId,
                FullName = $"{student.User?.FirstName} {student.User?.LastName}",
                DepartmentName = student.Department?.Name ?? "Unknown",
                CardId = student.CardId ?? "Not Assigned",
                TotalAttendance = totalAttendance,
                TotalClasses = totalClasses,
                AttendanceRate = totalClasses > 0 ? (decimal)totalAttendance / totalClasses * 100 : 0,
                Subjects = subjects
            };
        }
    }
}