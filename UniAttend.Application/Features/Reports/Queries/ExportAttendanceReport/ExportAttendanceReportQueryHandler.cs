using Microsoft.Extensions.Logging;
using MediatR;
using UniAttend.Core.Interfaces.Services;

namespace UniAttend.Application.Features.Reports.Queries.ExportAttendanceReport
{
    public class ExportAttendanceReportQueryHandler : IRequestHandler<ExportAttendanceReportQuery, byte[]>
    {
        private readonly IPrintService _printService;
        private readonly ILogger<ExportAttendanceReportQueryHandler> _logger;

        public ExportAttendanceReportQueryHandler(
            IPrintService printService,
            ILogger<ExportAttendanceReportQueryHandler> logger)
        {
            _printService = printService;
            _logger = logger;
        }

        public async Task<byte[]> Handle(
            ExportAttendanceReportQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await _printService.GenerateAttendanceReportPdfAsync(
                    request.StudyGroupId,
                    request.StartDate,
                    request.EndDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF report for group {StudyGroupId}", request.StudyGroupId);
                throw;
            }
        }
    }
}