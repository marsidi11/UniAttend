using MediatR;
using UniAttend.Application.Features.StudyGroups.DTOs;

namespace UniAttend.Application.Features.StudyGroups.Queries.GetGroupStudents
{
    public record GetGroupStudentsQuery : IRequest<IEnumerable<GroupStudentDto>>
    {
        public int GroupId { get; init; }
    }
}