using MediatR;
using Microsoft.Extensions.Logging;
using UniAttend.Application.Features.Reports.DTOs;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.Reports.Queries.GetAttendanceReport
{
    public class GetAttendanceReportQueryHandler : IRequestHandler<GetAttendanceReportQuery, AttendanceReportDto>
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<GetAttendanceReportQueryHandler> _logger;

        public GetAttendanceReportQueryHandler(
            IReportRepository reportRepository,
            ILogger<GetAttendanceReportQueryHandler> logger)
        {
            _reportRepository = reportRepository;
            _logger = logger;
        }

        public async Task<AttendanceReportDto> Handle(
            GetAttendanceReportQuery request, 
            CancellationToken cancellationToken)
        {
            var records = await _reportRepository.GetAttendanceRecordsAsync(
                request.StartDate,
                request.EndDate,
                request.DepartmentId,
                request.SubjectId,
                request.StudyGroupId,
                cancellationToken);

            var dailyRecords = records
                .GroupBy(r => r.CheckInTime.Date)
                .Select(g => new DailyAttendanceDto
                {
                    Date = g.Key,
                    TotalClasses = g.Select(r => r.CourseId).Distinct().Count(),
                    PresentStudents = g.Count(r => r.IsConfirmed),
                    AbsentStudents = g.Count(r => !r.IsConfirmed),
                    AttendanceRate = g.Any() ? 
                        (decimal)g.Count(r => r.IsConfirmed) / g.Count() * 100 : 0
                })
                .OrderBy(d => d.Date)
                .ToList();

            return new AttendanceReportDto
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TotalStudents = records.Select(r => r.StudentId).Distinct().Count(),
                TotalClasses = records.Select(r => r.CourseId).Distinct().Count(),
                OverallAttendance = records.Any() ? 
                    (decimal)records.Count(r => r.IsConfirmed) / records.Count() * 100 : 0,
                DailyRecords = dailyRecords
            };
        }
    }
}