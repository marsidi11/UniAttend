using MediatR;
using UniAttend.Application.Features.StudyGroups.DTOs;

namespace UniAttend.Application.Features.StudyGroups.Queries.GetStudyGroupStudents
{
    public record GetStudyGroupStudentsQuery : IRequest<IEnumerable<GroupStudentDto>>
    {
        public int StudyGroupId { get; init; }
    }
}