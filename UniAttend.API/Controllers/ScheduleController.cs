using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniAttend.Application.Features.Schedule.Queries.GetAllSchedules;
using UniAttend.Application.Features.Schedule.Commands.CreateSchedule;
using UniAttend.Application.Features.Schedule.Commands.UpdateSchedule;
using UniAttend.Application.Features.Schedule.Commands.DeleteSchedule;
using UniAttend.Application.Features.Schedule.Queries.GetClassroomSchedule;
using UniAttend.Application.Features.Schedule.Queries.GetGroupSchedule;
using UniAttend.Application.Features.Schedule.Queries.GetProfessorSchedule;
using UniAttend.Application.Features.Schedule.DTOs;
using UniAttend.Application.Features.Schedule.Queries.GetStudentSchedule;

namespace UniAttend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ScheduleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllSchedulesQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("professor/{professorId}")]
        [Authorize(Roles = "Admin,Secretary,Professor")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetByProfessor(
            int professorId,
            CancellationToken cancellationToken)
        {
            var query = new GetProfessorScheduleQuery { ProfessorId = professorId };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("student/{studentId}")]
        [Authorize(Roles = "Admin,Secretary,Student")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetByStudent(
            int studentId,
            CancellationToken cancellationToken)
        {
            var query = new GetStudentScheduleQuery { StudentId = studentId };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("classroom/{classroomId}")]
        [Authorize(Roles = "Admin,Secretary,Professor")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetByClassroom(
            int classroomId, 
            CancellationToken cancellationToken)
        {
            var query = new GetClassroomScheduleQuery { ClassroomId = classroomId };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("group/{studyGroupId}")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetByGroup(
            int studyGroupId, 
            CancellationToken cancellationToken)
        {
            var query = new GetGroupScheduleQuery { StudyGroupId = studyGroupId };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<ActionResult<int>> Create(
            [FromBody] CreateScheduleCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetByGroup), new { studyGroupId = command.StudyGroupId }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateScheduleCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteScheduleCommand { Id = id };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}