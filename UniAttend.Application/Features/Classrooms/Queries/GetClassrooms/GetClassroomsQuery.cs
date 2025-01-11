using MediatR;
using UniAttend.Application.Features.Classrooms.DTOs;

namespace UniAttend.Application.Features.Classrooms.Queries.GetClassrooms
{
    public record GetClassroomsQuery : IRequest<IEnumerable<ClassroomDto>>;
}