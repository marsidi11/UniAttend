using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniAttend.Application.Features.StudyGroups.Commands.CreateStudyGroup;
using UniAttend.Application.Features.StudyGroups.Commands.UpdateStudyGroup;
using UniAttend.Application.Features.StudyGroups.Commands.EnrollStudents;
using UniAttend.Application.Features.StudyGroups.Commands.RemoveStudentFromStudyGroup;
using UniAttend.Application.Features.StudyGroups.Commands.TransferStudent;
using UniAttend.Application.Features.StudyGroups.Queries.GetStudyGroupStudents;
using UniAttend.Application.Features.StudyGroups.Queries.GetProfessorStudyGroups;
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
        public async Task<ActionResult<IEnumerable<StudyGroupDto>>> GetProfessorStudyGroups(
            int professorId,
            [FromQuery] int? academicYearId,
            CancellationToken cancellationToken)
        {
            var query = new GetProfessorStudyGroupsQuery
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
            var query = new GetStudyGroupStudentsQuery { StudyGroupId = id };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<ActionResult<StudyGroupDto>> Create(
            [FromBody] CreateStudyGroupCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetStudents), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateStudyGroupCommand command,
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
            var command = new RemoveStudentFromStudyGroupCommand
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