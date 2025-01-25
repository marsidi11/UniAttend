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
        private readonly IStudyGroupRepository _studyGroupRepository;
        private readonly IAttendanceRecordRepository _attendanceRepository;
        private readonly ILogger<GetDepartmentReportQueryHandler> _logger;

        public GetDepartmentReportQueryHandler(
            IDepartmentRepository departmentRepository,
            IStudyGroupRepository studyGroupRepository,
            IAttendanceRecordRepository attendanceRepository,
            ILogger<GetDepartmentReportQueryHandler> logger)
        {
            _departmentRepository = departmentRepository;
            _studyGroupRepository = studyGroupRepository;
            _attendanceRepository = attendanceRepository;
            _logger = logger;
        }

        public async Task<DepartmentReportDto> Handle(GetDepartmentReportQuery request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByIdAsync(request.DepartmentId, cancellationToken)
                ?? throw new NotFoundException($"Department with ID {request.DepartmentId} not found");

            var studyGroups = await _studyGroupRepository.GetByDepartmentIdAsync(
                request.DepartmentId,
                request.AcademicYearId,
                cancellationToken);

            var studyGroupSummaries = new List<StudyGroupSummaryDto>();
            var totalStudents = 0;
            decimal totalAttendance = 0;

            foreach (var studyGroup in studyGroups)
            {
                var stats = await _attendanceRepository.GetGroupAttendanceReportAsync(
                    studyGroup.Id,
                    null,
                    null,
                    cancellationToken);

                totalStudents += studyGroup.Students.Count;
                totalAttendance += stats.OverallAttendance;

                studyGroupSummaries.Add(new StudyGroupSummaryDto
                {
                    StudyGroupId = studyGroup.Id,
                    StudyGroupName = studyGroup.Name,
                    SubjectName = studyGroup.Subject?.Name ?? "Unknown",
                    EnrolledStudents = studyGroup.Students.Count,
                    AttendanceRate = stats.OverallAttendance
                });
            }

            return new DepartmentReportDto
            {
                DepartmentId = department.Id,
                DepartmentName = department.Name,
                TotalGroups = studyGroups.Count(),
                TotalStudents = totalStudents,
                TotalSubjects = studyGroups.Select(g => g.SubjectId).Distinct().Count(),
                AverageAttendance = studyGroups.Any() ? totalAttendance / studyGroups.Count() : 0,
                Groups = studyGroupSummaries
            };
        }
    }
}