using MediatR;
using UniAttend.Application.Features.Classes.DTOs;

namespace UniAttend.Application.Features.Classes.Queries.GetActiveClasses
{
    public record GetActiveClassesQuery : IRequest<IEnumerable<ClassDto>>
    {
        public int? StudyGroupId { get; init; }
        public int? ClassroomId { get; init; }
        public DateTime? Date { get; init; }
    }
}