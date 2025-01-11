using MediatR;

namespace UniAttend.Application.Features.AcademicYears.Commands.UpdateAcademicYear
{
    public record UpdateAcademicYearCommand : IRequest<Unit>
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public bool? IsActive { get; init; }
    }
}