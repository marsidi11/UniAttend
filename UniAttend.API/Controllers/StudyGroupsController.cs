using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniAttend.Application.Features.StudyGroups.Commands.CreateGroup;
using UniAttend.Application.Features.StudyGroups.Commands.UpdateGroup;
using UniAttend.Application.Features.StudyGroups.Commands.EnrollStudents;
using UniAttend.Application.Features.StudyGroups.Commands.RemoveStudentFromGroup;
using UniAttend.Application.Features.StudyGroups.Commands.TransferStudent;
using UniAttend.Application.Features.StudyGroups.Queries.GetGroupStudents;
using UniAttend.Application.Features.StudyGroups.Queries.GetProfessorGroups;
using UniAttend.Application.Features.StudyGroups.Queries.GetStudyGroups;
using UniAttend.Application.Features.StudyGroups.Queries.GetStudyGroupById;
using UniAttend.Application.Features.StudyGroups.DTOs;

namespace UniAttend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StudyGroupsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudyGroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
[Authorize(Roles = "Admin,Secretary,Professor")] 
public async Task<ActionResult<IEnumerable<StudyGroupDto>>> GetAll(
    [FromQuery] int? academicYearId,
    CancellationToken cancellationToken)
{
    var query = new GetStudyGroupsQuery { AcademicYearId = academicYearId };
    var result = await _mediator.Send(query, cancellationToken);
    return Ok(result);
}

        [HttpGet("{id}")]
[Authorize(Roles = "Admin,Secretary,Professor")]
public async Task<ActionResult<StudyGroupDto>> GetById(
    int id,
    CancellationToken cancellationToken)
{
    var query = new GetStudyGroupByIdQuery { Id = id };
    var result = await _mediator.Send(query, cancellationToken);
    return result != null ? Ok(result) : NotFound();
}

        [HttpGet("professor/{professorId}")]
        [Authorize(Roles = "Admin,Secretary,Professor")]
        public async Task<ActionResult<IEnumerable<StudyGroupDto>>> GetProfessorGroups(
            int professorId,
            [FromQuery] int? academicYearId,
            CancellationToken cancellationToken)
        {
            var query = new GetProfessorGroupsQuery 
            { 
                ProfessorId = professorId,
                AcademicYearId = academicYearId
            };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}/students")]
        public async Task<ActionResult<IEnumerable<GroupStudentDto>>> GetStudents(
            int id, 
            CancellationToken cancellationToken)
        {
            var query = new GetGroupStudentsQuery { StudyGroupId = id };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<ActionResult<StudyGroupDto>> Create(
            [FromBody] CreateGroupCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetStudents), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateGroupCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/students/enroll")]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<IActionResult> EnrollStudents(
            int id, 
            [FromBody] EnrollStudentsCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.StudyGroupId)
                return BadRequest();

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{studyGroupId}/students/{studentId}")]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<IActionResult> RemoveStudent(
            int studyGroupId,
            int studentId,
            CancellationToken cancellationToken)
        {
            var command = new RemoveStudentFromGroupCommand 
            { 
                StudyGroupId = studyGroupId,
                StudentId = studentId
            };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPost("transfer-student")]
        [Authorize(Roles = "Admin,Secretary,Professor")]
        public async Task<IActionResult> TransferStudent(
            [FromBody] TransferStudentCommand command,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}