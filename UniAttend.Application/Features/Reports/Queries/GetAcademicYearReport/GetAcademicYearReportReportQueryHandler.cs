using MediatR;
using Microsoft.Extensions.Logging;
using UniAttend.Shared.Exceptions;
using UniAttend.Application.Features.Reports.DTOs;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.Reports.Queries.GetAcademicYearReport
{
    public class GetAcademicYearReportQueryHandler : IRequestHandler<GetAcademicYearReportQuery, AcademicYearReportDto>
    {
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IAttendanceRecordRepository _attendanceRepository;
        private readonly ILogger<GetAcademicYearReportQueryHandler> _logger;

        public GetAcademicYearReportQueryHandler(
            IAcademicYearRepository academicYearRepository,
            IAttendanceRecordRepository attendanceRepository,
            ILogger<GetAcademicYearReportQueryHandler> logger)
        {
            _academicYearRepository = academicYearRepository;
            _attendanceRepository = attendanceRepository;
            _logger = logger;
        }

        public async Task<AcademicYearReportDto> Handle(
            GetAcademicYearReportQuery request,
            CancellationToken cancellationToken)
        {
            var academicYear = await _academicYearRepository.GetByIdWithDetailsAsync(
                request.AcademicYearId,
                cancellationToken) ?? throw new NotFoundException($"Academic year {request.AcademicYearId} not found");

            var totalStudents = academicYear.StudyGroups.Sum(g => g.Students.Count);
            var activeGroups = academicYear.StudyGroups.Count(g => g.IsActive);
            
            var attendanceReport = await _attendanceRepository.GetAcademicYearAttendanceReportAsync(
                request.AcademicYearId,
                cancellationToken);

            return new AcademicYearReportDto
            {
                AcademicYearId = academicYear.Id,
                Name = academicYear.Name,
                StartDate = academicYear.StartDate,
                EndDate = academicYear.EndDate,
                TotalStudents = totalStudents,
                TotalGroups = academicYear.StudyGroups.Count,
                ActiveGroups = activeGroups,
                OverallAttendance = attendanceReport.OverallAttendance,
                PendingAttendanceConfirmations = attendanceReport.PendingConfirmations
            };
        }
    }
}