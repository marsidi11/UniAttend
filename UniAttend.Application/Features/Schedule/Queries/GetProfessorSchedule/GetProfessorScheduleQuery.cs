using MediatR;
using UniAttend.Application.Features.Schedule.DTOs;

namespace UniAttend.Application.Features.Schedule.Queries.GetProfessorSchedule
{
    public class GetProfessorScheduleQuery : IRequest<IEnumerable<ScheduleDto>>
    {
        public int ProfessorId { get; set; }
    }
}