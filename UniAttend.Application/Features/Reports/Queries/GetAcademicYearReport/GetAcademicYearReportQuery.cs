using MediatR;
using UniAttend.Application.Features.Reports.DTOs;

namespace UniAttend.Application.Features.Reports.Queries.GetAcademicYearReport
{
    public record GetAcademicYearReportQuery : IRequest<AcademicYearReportDto>
    {
        public int AcademicYearId { get; init; }
    }
}