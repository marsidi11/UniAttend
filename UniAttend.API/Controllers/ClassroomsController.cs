using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniAttend.Application.Features.Classrooms.Commands.CreateClassroom;
using UniAttend.Application.Features.Classrooms.Commands.UpdateClassroom;
using UniAttend.Application.Features.Classrooms.Commands.AssignReader;
using UniAttend.Application.Features.Classrooms.Commands.RemoveReader;
using UniAttend.Application.Features.Classrooms.Queries.GetClassrooms;
using UniAttend.Application.Features.Classrooms.Queries.GetClassroomById;
using UniAttend.Application.Features.Classrooms.Queries.GetAvailableClassrooms;
using UniAttend.Application.Features.Classrooms.DTOs;

namespace UniAttend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClassroomsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClassroomsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassroomDto>>> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetClassroomsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClassroomDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var query = new GetClassroomByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<ActionResult<int>> Create(
            [FromBody] CreateClassroomCommand command,
            CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<ActionResult> Update(
            int id,
            [FromBody] UpdateClassroomCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/reader")]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<ActionResult> AssignReader(
            int id,
            [FromBody] AssignReaderCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.ClassroomId)
                return BadRequest();

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}/reader")]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<ActionResult> RemoveReader(
            int id,
            CancellationToken cancellationToken)
        {
            var command = new RemoveReaderCommand { ClassroomId = id };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpGet("available")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ClassroomDto>>> GetAvailable(
            [FromQuery] DateTime startTime,
            [FromQuery] DateTime endTime,
            CancellationToken cancellationToken)
        {
            var query = new GetAvailableClassroomsQuery 
            { 
                StartTime = startTime,
                EndTime = endTime
            };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}