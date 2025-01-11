using MediatR;
using UniAttend.Application.Features.AcademicYears.DTOs;

namespace UniAttend.Application.Features.AcademicYears.Commands.CreateAcademicYear
{
    public record CreateAcademicYearCommand : IRequest<AcademicYearDto>
    {
        public string Name { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}