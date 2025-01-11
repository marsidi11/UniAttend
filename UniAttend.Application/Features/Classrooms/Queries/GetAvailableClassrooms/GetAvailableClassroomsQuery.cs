using MediatR;
using UniAttend.Application.Features.Classrooms.DTOs;

namespace UniAttend.Application.Features.Classrooms.Queries.GetAvailableClassrooms
{
    public record GetAvailableClassroomsQuery : IRequest<IEnumerable<ClassroomDto>>
    {
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
    }
}