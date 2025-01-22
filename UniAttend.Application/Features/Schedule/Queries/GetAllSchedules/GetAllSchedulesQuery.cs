using MediatR;
using UniAttend.Application.Features.Schedule.DTOs;

namespace UniAttend.Application.Features.Schedule.Queries.GetAllSchedules
{
    public class GetAllSchedulesQuery : IRequest<IEnumerable<ScheduleDto>>
    {
    }
}