using MediatR;
using UniAttend.Application.Features.AcademicYears.DTOs;

namespace UniAttend.Application.Features.AcademicYears.Queries.GetAcademicYears
{
    public record GetAcademicYearsQuery : IRequest<IEnumerable<AcademicYearDto>>;
}