using MediatR;

namespace UniAttend.Application.Features.AcademicYears.Commands.CloseAcademicYear
{
    public record CloseAcademicYearCommand : IRequest<Unit>
    {
        public int Id { get; init; }
    }
}