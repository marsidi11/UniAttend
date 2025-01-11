using MediatR;
using UniAttend.Application.Features.AcademicYears.DTOs;

namespace UniAttend.Application.Features.AcademicYears.Queries.GetActiveAcademicYear
{
    public record GetActiveAcademicYearQuery : IRequest<AcademicYearDto?>;
}