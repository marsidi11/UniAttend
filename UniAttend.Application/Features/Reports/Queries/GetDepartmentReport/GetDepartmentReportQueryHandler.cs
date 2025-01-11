using MediatR;
using Microsoft.Extensions.Logging;
using UniAttend.Shared.Exceptions;
using UniAttend.Application.Features.Reports.DTOs;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.Reports.Queries.GetDepartmentReport
{
    public class GetDepartmentReportQueryHandler : IRequestHandler<GetDepartmentReportQuery, DepartmentReportDto>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IStudyGroupRepository _groupRepository;
        private readonly IAttendanceRecordRepository _attendanceRepository;
        private readonly ILogger<GetDepartmentReportQueryHandler> _logger;

        public GetDepartmentReportQueryHandler(
            IDepartmentRepository departmentRepository,
            IStudyGroupRepository groupRepository,
            IAttendanceRecordRepository attendanceRepository,
            ILogger<GetDepartmentReportQueryHandler> logger)
        {
            _departmentRepository = departmentRepository;
            _groupRepository = groupRepository;
            _attendanceRepository = attendanceRepository;
            _logger = logger;
        }

        public async Task<DepartmentReportDto> Handle(GetDepartmentReportQuery request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByIdAsync(request.DepartmentId, cancellationToken)
                ?? throw new NotFoundException($"Department with ID {request.DepartmentId} not found");

            var groups = await _groupRepository.GetByDepartmentIdAsync(
                request.DepartmentId,
                request.AcademicYearId,
                cancellationToken);

            var groupSummaries = new List<GroupSummaryDto>();
            var totalStudents = 0;
            decimal totalAttendance = 0;

            foreach (var group in groups)
            {
                var stats = await _attendanceRepository.GetGroupAttendanceReportAsync(
                    group.Id,
                    null,
                    null,
                    cancellationToken);

                totalStudents += group.Students.Count;
                totalAttendance += stats.OverallAttendance;

                groupSummaries.Add(new GroupSummaryDto
                {
                    GroupId = group.Id,
                    GroupName = group.Name,
                    SubjectName = group.Subject?.Name ?? "Unknown",
                    EnrolledStudents = group.Students.Count,
                    AttendanceRate = stats.OverallAttendance
                });
            }

            return new DepartmentReportDto
            {
                DepartmentId = department.Id,
                DepartmentName = department.Name,
                TotalGroups = groups.Count(),
                TotalStudents = totalStudents,
                TotalSubjects = groups.Select(g => g.SubjectId).Distinct().Count(),
                AverageAttendance = groups.Any() ? totalAttendance / groups.Count() : 0,
                Groups = groupSummaries
            };
        }
    }
}