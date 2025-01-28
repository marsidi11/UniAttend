using MediatR;
using Microsoft.Extensions.Logging;
using UniAttend.Shared.Exceptions;
using UniAttend.Application.Features.Reports.DTOs;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.Reports.Queries.GetGroupReport
{
    public class GetGroupReportQueryHandler : IRequestHandler<GetGroupReportQuery, GroupReportDto>
    {
        private readonly IStudyGroupRepository _studyGroupRepository;
        private readonly IAttendanceRecordRepository _attendanceRepository;
        private readonly ILogger<GetGroupReportQueryHandler> _logger;

        public GetGroupReportQueryHandler(
            IStudyGroupRepository studyGroupRepository,
            IAttendanceRecordRepository attendanceRepository,
            ILogger<GetGroupReportQueryHandler> logger)
        {
            _studyGroupRepository = studyGroupRepository;
            _attendanceRepository = attendanceRepository;
            _logger = logger;
        }

        public async Task<GroupReportDto> Handle(GetGroupReportQuery request, CancellationToken cancellationToken)
        {
            var studyGroup = await _studyGroupRepository.GetByIdWithDetailsAsync(request.StudyGroupId, cancellationToken)
                ?? throw new NotFoundException($"StudyGroup with ID {request.StudyGroupId} not found");

            var attendanceStats = await _attendanceRepository.GetGroupAttendanceReportAsync(
                request.StudyGroupId,
                request.StartDate,
                request.EndDate,
                cancellationToken);

            var studentAttendance = new List<StudentAttendanceDto>();
            foreach (var student in studyGroup.Students)
            {
                var stats = await _attendanceRepository.GetStudentGroupAttendanceAsync(
                    student.StudentId,
                    request.StudyGroupId,
                    request.StartDate,
                    request.EndDate,
                    cancellationToken);

                studentAttendance.Add(new StudentAttendanceDto
                {
                    StudentId = student.StudentId,
                    StudentNumber = student.Student?.StudentId ?? "Unknown",
                    FullName = $"{student.Student?.User?.FirstName} {student.Student?.User?.LastName}",
                    AttendedCourseSessions = stats.AttendedCourseSessions,
                    AttendanceRate = stats.TotalCourseSessions > 0
                        ? (decimal)stats.AttendedCourseSessions / stats.TotalCourseSessions * 100
                        : 0
                });
            }

            return new GroupReportDto
            {
                StudyGroupId = studyGroup.Id,
                StudyGroupName = studyGroup.Name,
                SubjectName = studyGroup.Subject?.Name ?? "Unknown",
                ProfessorName = $"{studyGroup.Professor?.User?.FirstName} {studyGroup.Professor?.User?.LastName}",
                TotalStudents = studyGroup.Students.Count,
                TotalCourseSessions = attendanceStats.TotalCourseSessions,
                AverageAttendance = attendanceStats.OverallAttendance,
                Students = studentAttendance
            };
        }
    }
}