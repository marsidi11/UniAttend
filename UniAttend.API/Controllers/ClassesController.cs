using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniAttend.Application.Features.Classes.Commands.OpenClass;
using UniAttend.Application.Features.Classes.Commands.CloseClass;
using UniAttend.Application.Features.Classes.Queries.GetActiveClasses;
using UniAttend.Application.Features.Classes.DTOs;

namespace UniAttend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClassesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all active classes with optional filtering
        /// </summary>
        /// <param name="groupId">Optional group ID filter</param>
        /// <param name="classroomId">Optional classroom ID filter</param>
        /// <param name="date">Optional date filter</param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetActiveClasses(
            [FromQuery] int? groupId,
            [FromQuery] int? classroomId,
            [FromQuery] DateTime? date,
            CancellationToken cancellationToken)
        {
            var query = new GetActiveClassesQuery
            {
                GroupId = groupId,
                ClassroomId = classroomId,
                Date = date
            };

            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Opens a new class session
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Professor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ClassDto>> OpenClass(
            [FromBody] OpenClassCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(
                nameof(GetActiveClasses),
                new { id = result.Id },
                result);
        }

        /// <summary>
        /// Closes an active class session
        /// </summary>
        /// <param name="id">The ID of the class to close</param>
        [HttpPost("{id}/close")]
        [Authorize(Roles = "Professor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> CloseClass(
            int id,
            CancellationToken cancellationToken)
        {
            var command = new CloseClassCommand { ClassId = id };
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Gets a class by its ID
        /// </summary>
        /// <param name="id">The ID of the class</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassDto>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var query = new GetActiveClassesQuery { GroupId = id };
            var result = await _mediator.Send(query, cancellationToken);
            var classDto = result.FirstOrDefault();

            if (classDto == null)
                return NotFound();

            return Ok(classDto);
        }

        /// <summary>
        /// Gets all classes for a specific group
        /// </summary>
        /// <param name="groupId">The ID of the group</param>
        [HttpGet("group/{groupId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetByGroup(
            int groupId,
            CancellationToken cancellationToken)
        {
            var query = new GetActiveClassesQuery { GroupId = groupId };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Gets all classes in a specific classroom
        /// </summary>
        /// <param name="classroomId">The ID of the classroom</param>
        [HttpGet("classroom/{classroomId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetByClassroom(
            int classroomId,
            CancellationToken cancellationToken)
        {
            var query = new GetActiveClassesQuery { ClassroomId = classroomId };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}