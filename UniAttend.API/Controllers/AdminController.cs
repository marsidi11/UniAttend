using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using UniAttend.Application.Features.Departments.Commands.CreateDepartment;
using UniAttend.Application.Features.Departments.Commands.UpdateDepartment;
using UniAttend.Application.Features.AcademicYears.Commands.CreateAcademicYear;
using UniAttend.Application.Features.Users.Commands.CreateStaff;
using UniAttend.Application.Features.Subjects.Commands.CreateSubject;
using UniAttend.Application.Features.Reports.DTOs;
using UniAttend.Application.Features.Reports.Queries.GetAttendanceReport;

namespace UniAttend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("departments")]
        public async Task<ActionResult<int>> CreateDepartment(CreateDepartmentCommand command)
            => Ok(await _mediator.Send(command));

        [HttpPut("departments/{id}")]
        public async Task<ActionResult> UpdateDepartment(int id, UpdateDepartmentCommand command)
        {
            if (id != command.Id) return BadRequest();
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("academic-years")]
        public async Task<ActionResult<int>> CreateAcademicYear(CreateAcademicYearCommand command)
            => Ok(await _mediator.Send(command));

        [HttpPost("subjects")]
        public async Task<ActionResult<int>> CreateSubject(CreateSubjectCommand command)
            => Ok(await _mediator.Send(command));

        [HttpPost("staff")]
        public async Task<ActionResult<int>> CreateStaffMember(CreateStaffCommand command)
            => Ok(await _mediator.Send(command));

        [HttpGet("reports/attendance")]
        public async Task<ActionResult<AttendanceReportDto>> GetAttendanceReport([FromQuery] GetAttendanceReportQuery query)
            => Ok(await _mediator.Send(query));
    }
}