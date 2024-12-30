using MediatR;

namespace UniAttend.Application.Features.AcademicYears.Commands.CreateAcademicYear
{
    public record CreateAcademicYearCommand : IRequest<int>
    {
        public string Name { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}