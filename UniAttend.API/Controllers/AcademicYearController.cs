using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using UniAttend.Application.Features.AcademicYears.Commands.CreateAcademicYear;
using UniAttend.Application.Features.AcademicYears.Commands.UpdateAcademicYear;
using UniAttend.Application.Features.AcademicYears.Commands.CloseAcademicYear;
using UniAttend.Application.Features.AcademicYears.Queries.GetAcademicYears;
using UniAttend.Application.Features.AcademicYears.Queries.GetActiveAcademicYear;
using UniAttend.Application.Features.Groups.DTOs;
using UniAttend.Application.Features.AcademicYears.DTOs;

namespace UniAttend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AcademicYearController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AcademicYearController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<ActionResult<IEnumerable<AcademicYearDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAcademicYearsQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("active")]
        [Authorize]
        public async Task<ActionResult<AcademicYearDto>> GetActive(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetActiveAcademicYearQuery(), cancellationToken);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AcademicYearDto>> Create([FromBody] CreateAcademicYearCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetAll), new { id }, command);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAcademicYearCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/close")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Close(int id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new CloseAcademicYearCommand { Id = id }, cancellationToken);
            return NoContent();
        }
    }
}