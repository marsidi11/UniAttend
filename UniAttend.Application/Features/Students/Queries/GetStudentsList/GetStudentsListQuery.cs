using MediatR;
using UniAttend.Application.Features.Students.DTOs;

namespace UniAttend.Application.Features.Students.Queries.GetStudentsList
{
    public class GetStudentsListQuery : IRequest<List<StudentListDto>>
    {
    }
}