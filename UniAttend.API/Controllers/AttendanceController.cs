using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using UniAttend.Application.Features.Attendance.Commands.ConfirmAttendance;
using UniAttend.Application.Features.Attendance.Commands.RecordCardAttendance;
using UniAttend.Application.Features.Attendance.Commands.RecordOtpAttendance;
using UniAttend.Application.Features.Attendance.Queries.GetClassAttendance;
using UniAttend.Application.Features.Attendance.Queries.GetStudentAttendance;
using UniAttend.Application.Features.Attendance.DTOs;
using UniAttend.API.Extensions;

namespace UniAttend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private readonly IMediator _mediator;
    
        public AttendanceController(IMediator mediator)
        {
            _mediator = mediator;
        }
    
        [HttpPost("card")]
        public async Task<IActionResult> RecordCardAttendance(RecordCardAttendanceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    
        [HttpPost("otp")]
        public async Task<IActionResult> RecordOtpAttendance(RecordOtpAttendanceCommand command)
        {
            command.StudentId = User.GetUserId();
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    
        [Authorize(Policy = "RequireProfessorRole")]
        [HttpPost("classes/{classId}/confirm")]
        public async Task<IActionResult> ConfirmAttendance(int classId)
        {
            var command = new ConfirmAttendanceCommand 
            { 
                ClassId = classId,
                ProfessorId = User.GetUserId()
            };
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
    /// Gets attendance records for a specific class
    /// </summary>
    [HttpGet("classes/{classId}")]
    [Authorize(Policy = "RequireProfessorRole")]
    public async Task<ActionResult<IEnumerable<AttendanceRecordDto>>> GetClassAttendance(
        int classId,
        [FromQuery] DateTime? date,
        CancellationToken cancellationToken)
    {
        var query = new GetClassAttendanceQuery 
        { 
            ClassId = classId,
            Date = date
        };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets attendance records for the authenticated student
    /// </summary>
    [HttpGet("student")]
    [Authorize(Policy = "RequireStudentRole")]
    public async Task<ActionResult<IEnumerable<AttendanceRecordDto>>> GetStudentAttendance(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        CancellationToken cancellationToken)
    {
        var query = new GetStudentAttendanceQuery
        {
            StudentId = User.GetUserId(),
            StartDate = startDate,
            EndDate = endDate
        };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
    }
}