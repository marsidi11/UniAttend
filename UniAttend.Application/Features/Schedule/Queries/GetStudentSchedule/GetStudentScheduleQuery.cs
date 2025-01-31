using MediatR;
using UniAttend.Application.Features.Schedule.DTOs;

namespace UniAttend.Application.Features.Schedule.Queries.GetStudentSchedule
{
    public class GetStudentScheduleQuery : IRequest<IEnumerable<ScheduleDto>>
    {
        public int StudentId { get; set; }
    }
}