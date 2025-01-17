using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniAttend.Application.Features.Subjects.Commands.CreateSubject;
using UniAttend.Application.Features.Subjects.Commands.UpdateSubject;
using UniAttend.Application.Features.Subjects.Commands.DeactivateSubject;
using UniAttend.Application.Features.Subjects.Queries.GetDepartmentSubjects;
using UniAttend.Application.Features.Subjects.Queries.GetSubjectById;
using UniAttend.Application.Features.Subjects.DTOs;

namespace UniAttend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Secretary,Professor")]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetAll(
            [FromQuery] int? departmentId,
            [FromQuery] bool? isActive,
            CancellationToken cancellationToken)
        {
            var query = new GetDepartmentSubjectsQuery 
            { 
                DepartmentId = departmentId ?? 0,
                IsActive = isActive
            };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Secretary,Professor")]
        public async Task<ActionResult<SubjectDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var query = new GetSubjectByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SubjectDto>> Create(
            [FromBody] CreateSubjectCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateSubjectCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var command = new DeactivateSubjectCommand { Id = id };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}