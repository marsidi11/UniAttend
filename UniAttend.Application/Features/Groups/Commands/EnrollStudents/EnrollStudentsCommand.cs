using MediatR;

namespace UniAttend.Application.Features.Groups.Commands.EnrollStudents
{
    public record EnrollStudentsCommand : IRequest<Unit>
    {
        public int GroupId { get; init; }
        public IEnumerable<int> StudentIds { get; init; } = new List<int>();
    }
}