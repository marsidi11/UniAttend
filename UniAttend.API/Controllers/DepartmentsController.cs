using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniAttend.Application.Features.Departments.Commands.CreateDepartment;
using UniAttend.Application.Features.Departments.Commands.UpdateDepartment;
using UniAttend.Application.Features.Departments.DTOs;
using UniAttend.Application.Features.Departments.Queries.GetDepartmentById;
using UniAttend.Application.Features.Departments.Queries.GetDepartments;

namespace UniAttend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all departments. Available to all authenticated users.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
        {
            var query = new GetDepartmentsQuery();
            var departments = await _mediator.Send(query);
            return Ok(departments);
        }

        /// <summary>
        /// Gets a specific department by ID. Available to all authenticated users.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentById(int id)
        {
            var query = new GetDepartmentByIdQuery { Id = id };
            var department = await _mediator.Send(query);
            return Ok(department);
        }

        /// <summary>
        /// Creates a new department. Admin only.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> CreateDepartment([FromBody] CreateDepartmentCommand command)
        {
            var departmentId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = departmentId }, departmentId);
        }

        /// <summary>
        /// Updates an existing department. Admin only.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Toggles department active status. Admin only.
        /// </summary>
        [HttpPatch("{id}/toggle-status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ToggleDepartmentStatus(int id)
        {
            // First get the current department to check its status
            var query = new GetDepartmentByIdQuery { Id = id };
            var department = await _mediator.Send(query);

            var command = new UpdateDepartmentCommand
            {
                Id = id,
                Name = department.Name,
                Description = string.Empty,
                IsActive = !department.IsActive // Toggle the current status
            };

            await _mediator.Send(command);
            return NoContent();
        }
    }
}