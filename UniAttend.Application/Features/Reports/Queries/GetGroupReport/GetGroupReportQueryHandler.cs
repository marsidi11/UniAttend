using MediatR;
using Microsoft.Extensions.Logging;
using UniAttend.Shared.Exceptions;
using UniAttend.Application.Features.Reports.DTOs;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.Reports.Queries.GetGroupReport
{
    public class GetGroupReportQueryHandler : IRequestHandler<GetGroupReportQuery, GroupReportDto>
    {
        private readonly IStudyGroupRepository _groupRepository;
        private readonly IAttendanceRecordRepository _attendanceRepository;
        private readonly ILogger<GetGroupReportQueryHandler> _logger;

        public GetGroupReportQueryHandler(
            IStudyGroupRepository groupRepository,
            IAttendanceRecordRepository attendanceRepository,
            ILogger<GetGroupReportQueryHandler> logger)
        {
            _groupRepository = groupRepository;
            _attendanceRepository = attendanceRepository;
            _logger = logger;
        }

        public async Task<GroupReportDto> Handle(GetGroupReportQuery request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdWithDetailsAsync(request.GroupId, cancellationToken)
                ?? throw new NotFoundException($"Group with ID {request.GroupId} not found");

            var attendanceStats = await _attendanceRepository.GetGroupAttendanceReportAsync(
                request.GroupId,
                request.StartDate,
                request.EndDate,
                cancellationToken);

            var studentAttendance = new List<StudentAttendanceDto>();
            foreach (var student in group.Students)
            {
                var stats = await _attendanceRepository.GetStudentGroupAttendanceAsync(
                    student.StudentId,
                    request.GroupId,
                    request.StartDate,
                    request.EndDate,
                    cancellationToken);

                studentAttendance.Add(new StudentAttendanceDto
                {
                    StudentId = student.StudentId,
                    StudentNumber = student.Student?.StudentId ?? "Unknown",
                    FullName = $"{student.Student?.User?.FirstName} {student.Student?.User?.LastName}",
                    AttendedClasses = stats.AttendedClasses,
                    AttendanceRate = stats.TotalClasses > 0
                        ? (decimal)stats.AttendedClasses / stats.TotalClasses * 100
                        : 0
                });
            }

            return new GroupReportDto
            {
                GroupId = group.Id,
                GroupName = group.Name,
                SubjectName = group.Subject?.Name ?? "Unknown",
                ProfessorName = $"{group.Professor?.User?.FirstName} {group.Professor?.User?.LastName}",
                TotalStudents = group.Students.Count,
                TotalClasses = attendanceStats.TotalClasses,
                AverageAttendance = attendanceStats.OverallAttendance,
                Students = studentAttendance
            };
        }
    }
}