using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniAttend.Application.Features.CourseSessions.Commands.OpenCourseSession;
using UniAttend.Application.Features.CourseSessions.Commands.CloseCourseSession;
using UniAttend.Application.Features.CourseSessions.Queries.GetActiveCourseSessions;
using UniAttend.Application.Features.CourseSessions.DTOs;

namespace UniAttend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CourseSessionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CourseSessionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all Active Course Sessions with optional filtering
        /// </summary>
        /// <param name="studyGroupId">Optional group ID filter</param>
        /// <param name="classroomId">Optional classroom ID filter</param>
        /// <param name="date">Optional date filter</param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CourseSessionDto>>> GetActiveCourseSessions(
            [FromQuery] int? studyGroupId,
            [FromQuery] int? classroomId,
            [FromQuery] int? professorId,
            [FromQuery] DateTime? date,
            CancellationToken cancellationToken)
        {
            var query = new GetActiveCourseSessionsQuery
            {
                StudyGroupId = studyGroupId,
                ClassroomId = classroomId,
                ProfessorId = professorId,
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
        public async Task<ActionResult<CourseSessionDto>> OpenCourseSession(
            [FromBody] OpenCourseSessionCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(
                nameof(GetActiveCourseSessions),
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
            var command = new CloseCourseSessionCommand { CourseSessionId = id };
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
        public async Task<ActionResult<CourseSessionDto>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var query = new GetActiveCourseSessionsQuery { CourseSessionId = id };
            var result = await _mediator.Send(query, cancellationToken);
            var courseSessionDto = result.FirstOrDefault();
        
            if (courseSessionDto == null)
                return NotFound();
        
            return Ok(courseSessionDto);
        }

        /// <summary>
        /// Gets all courseSessions for a specific group
        /// </summary>
        /// <param name="studyGroupId">The ID of the group</param>
        [HttpGet("group/{studyGroupId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CourseSessionDto>>> GetByGroup(
            int studyGroupId,
            CancellationToken cancellationToken)
        {
            var query = new GetActiveCourseSessionsQuery { StudyGroupId = studyGroupId };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Gets all courseSessions in a specific classroom
        /// </summary>
        /// <param name="classroomId">The ID of the classroom</param>
        [HttpGet("classroom/{classroomId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CourseSessionDto>>> GetByClassroom(
            int classroomId,
            CancellationToken cancellationToken)
        {
            var query = new GetActiveCourseSessionsQuery { ClassroomId = classroomId };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}