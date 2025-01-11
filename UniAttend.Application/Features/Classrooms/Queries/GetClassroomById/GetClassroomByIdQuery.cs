using MediatR;
using UniAttend.Application.Features.Classrooms.DTOs;

namespace UniAttend.Application.Features.Classrooms.Queries.GetClassroomById
{
    public record GetClassroomByIdQuery : IRequest<ClassroomDto>
    {
        public int Id { get; init; }
    }
}