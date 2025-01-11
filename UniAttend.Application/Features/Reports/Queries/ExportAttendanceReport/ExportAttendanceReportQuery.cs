using MediatR;

namespace UniAttend.Application.Features.Reports.Queries.ExportAttendanceReport
{
    public record ExportAttendanceReportQuery : IRequest<byte[]>
    {
        public int GroupId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}