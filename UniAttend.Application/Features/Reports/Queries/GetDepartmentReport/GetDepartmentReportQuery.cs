using MediatR;
using UniAttend.Application.Features.Reports.DTOs;

namespace UniAttend.Application.Features.Reports.Queries.GetDepartmentReport
{
    public record GetDepartmentReportQuery : IRequest<DepartmentReportDto>
    {
        public int DepartmentId { get; init; }
        public int? AcademicYearId { get; init; }
    }
}