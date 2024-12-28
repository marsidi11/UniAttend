using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using UniAttend.API.Extensions;
using UniAttend.Application.Features.Students.Queries.GetStudentAbsenceAlerts;
using UniAttend.Application.Features.Students.Queries.GetStudentAttendance;
using UniAttend.Application.Features.Students.Queries.GetStudentGroups;

namespace UniAttend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("attendance")]
        public async Task<IActionResult> GetAttendanceRecords([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = new GetStudentAttendanceQuery
            {
                StudentId = User.GetUserId(),
                StartDate = startDate,
                EndDate = endDate
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("enrolled-groups")]
        public async Task<IActionResult> GetEnrolledGroups()
        {
            var query = new GetStudentGroupsQuery { StudentId = User.GetUserId() };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("absence-alerts")]
        public async Task<IActionResult> GetAbsenceAlerts()
        {
            var query = new GetStudentAbsenceAlertsQuery { StudentId = User.GetUserId() };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}