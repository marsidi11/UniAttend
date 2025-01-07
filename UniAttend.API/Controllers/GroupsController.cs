using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using UniAttend.Application.Features.Groups.DTOs;
using UniAttend.Application.Features.Groups.Commands.EnrollStudents;
using UniAttend.Application.Features.Groups.Commands.TransferStudent;
using UniAttend.Application.Features.Groups.Queries;

namespace UniAttend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}/students")]
        public async Task<ActionResult<IEnumerable<GroupStudentDto>>> GetStudents(int id)
        {
            var query = new GetGroupStudentsQuery { GroupId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("{id}/students/enroll")]
        [Authorize(Policy = "RequireProfessorRole")]
        public async Task<IActionResult> EnrollStudents(int id, [FromBody] EnrollStudentsRequest request)
        {
            var command = new EnrollStudentsCommand
            {
                GroupId = id,
                StudentIds = request.StudentIds
            };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("transfer-student")]
        [Authorize(Policy = "RequireProfessorRole")]
        public async Task<IActionResult> TransferStudent([FromBody] TransferStudentRequest request)
        {
            var command = new TransferStudentCommand
            {
                StudentId = request.StudentId,
                FromGroupId = request.FromGroupId,
                ToGroupId = request.ToGroupId
            };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{id}/attendance-stats")]
        public async Task<ActionResult<AttendanceStatsDto>> GetAttendanceStats(int id)
        {
            var query = new GetGroupAttendanceStatsQuery { GroupId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}/schedule")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetSchedule(int id)
        {
            var query = new GetGroupScheduleQuery { GroupId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}