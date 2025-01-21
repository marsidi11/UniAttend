using MediatR;

namespace UniAttend.Application.Features.StudyGroups.Commands.EnrollStudents
{
    public record EnrollStudentsCommand : IRequest<Unit>
    {
        public int StudyGroupId { get; init; }
        public IEnumerable<int> StudentIds { get; init; } = new List<int>();
    }
}