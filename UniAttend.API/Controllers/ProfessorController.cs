using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniAttend.Application.Features.Professors.Queries.GetProfessors;
using UniAttend.Application.Features.Professors.Queries.GetProfessorById;
using UniAttend.Application.Features.Professors.DTOs;

namespace UniAttend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfessorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<ActionResult<IEnumerable<ProfessorDto>>> GetAll(
            [FromQuery] int? departmentId,
            [FromQuery] bool? isActive,
            CancellationToken cancellationToken)
        {
            var query = new GetProfessorsQuery 
            { 
                DepartmentId = departmentId,
                IsActive = isActive
            };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<ActionResult<ProfessorDto>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var query = new GetProfessorByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);
            return result != null ? Ok(result) : NotFound();
        }
    }
}