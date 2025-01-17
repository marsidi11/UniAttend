using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using UniAttend.API.Extensions;
using UniAttend.Application.Features.Students.Queries.GetStudentAbsenceAlerts;
using UniAttend.Application.Features.Students.Queries.GetStudentAttendance;
using UniAttend.Application.Features.Students.Queries.GetStudentGroups;
using UniAttend.Application.Features.Students.Commands.AssignCard;
using UniAttend.Application.Features.Students.Commands.RemoveCard;
using UniAttend.Application.Features.Students.Commands.RegisterStudent;
using UniAttend.Application.Features.Students.Queries.GetStudentsList;

namespace UniAttend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetStudentsListQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Secretary")]
        public async Task<ActionResult<int>> RegisterStudent([FromBody] RegisterStudentCommand command)
        {
            var studentId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAll), new { id = studentId }, studentId);
        }

        [HttpGet("attendance")]
        public async Task<IActionResult> GetAttendanceRecords([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = new GetStudentAttendanceQuery
            {
                StudentId = User.GetUserId(),
                StartDate = startDate,
                EndDate = endDate
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("enrolled-groups")]
        public async Task<IActionResult> GetEnrolledGroups()
        {
            var query = new GetStudentGroupsQuery { StudentId = User.GetUserId() };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("absence-alerts")]
        public async Task<IActionResult> GetAbsenceAlerts()
        {
            var query = new GetStudentAbsenceAlertsQuery { StudentId = User.GetUserId() };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}/card")]
        public async Task<IActionResult> AssignCard(int id, [FromBody] AssignCardCommand command)
        {
            command.StudentId = id;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}/card")]
        public async Task<IActionResult> RemoveCard(int id)
        {
            var command = new RemoveCardCommand { StudentId = id };
            await _mediator.Send(command);
            return Ok();
        }
    }
}