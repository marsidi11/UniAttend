using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using UniAttend.Application.Features.Attendance.Commands.ConfirmAttendance;
using UniAttend.Application.Features.Attendance.Commands.RecordCardAttendance;
using UniAttend.Application.Features.Attendance.Commands.RecordOtpAttendance;
using UniAttend.Application.Features.Attendance.Queries.GetCourseSessionAttendance;
using UniAttend.Application.Features.Attendance.Queries.GetStudentAttendance;
using UniAttend.Application.Features.Attendance.DTOs;
using UniAttend.API.Extensions;
using UniAttend.Core.Enums;
using UniAttend.Application.Features.Attendance.Commands.MarkAbsent;
using UniAttend.Core.Interfaces.Services;

namespace UniAttend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly INetworkValidationService _networkValidationService;

        public AttendanceController(
            IMediator mediator,
            INetworkValidationService networkValidationService)
        {
            _mediator = mediator;
            _networkValidationService = networkValidationService ??
                throw new ArgumentNullException(nameof(networkValidationService));
        }

        [HttpPost("card")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> RecordCardAttendance(RecordCardAttendanceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("otp")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> RecordOtpAttendance(RecordOtpAttendanceCommand command)
        {
            // Get client IP
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(clientIp))
            {
                return BadRequest("Could not determine client IP address");
            }

            // Validate network
            if (!_networkValidationService.IsOnAllowedNetwork(clientIp))
            {
                return BadRequest("Must be on classroom network to check in");
            }

            command.StudentId = User.GetUserId();
            command.VerificationType = VerificationType.Totp;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Professor")]
        [HttpPost("courseSessions/{courseSessionId}/confirm")]
        public async Task<IActionResult> ConfirmAttendance(int courseSessionId)
        {
            var command = new ConfirmAttendanceCommand
            {
                CourseSessionId = courseSessionId,
            };
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Gets attendance records for a specific class
        /// </summary>
        [HttpGet("courseSessions/{courseSessionId}")]
        [Authorize(Roles = "Professor")]
        public async Task<ActionResult<IEnumerable<AttendanceRecordDto>>> GetCourseSessionAttendance(
            int courseSessionId,
            [FromQuery] DateTime? date,
            CancellationToken cancellationToken)
        {
            var query = new GetCourseSessionAttendanceQuery
            {
                CourseSessionId = courseSessionId,
                Date = date
            };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Gets attendance records for the authenticated student
        /// </summary>
        [HttpGet("student")]
        [Authorize(Roles = "Student, Secretary, Professor")]
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

        /// <summary>
        /// Marks a student as absent for a specific class
        /// </summary>
        [Authorize(Roles = "Professor")]
        [HttpPost("courseSessions/{courseSessionId}/students/{studentId}/absent")]
        public async Task<IActionResult> MarkAbsent(int courseSessionId, int studentId)
        {
            var command = new MarkAbsentCommand
            {
                CourseSessionId = courseSessionId,
                StudentId = studentId
            };
            await _mediator.Send(command);
            return Ok();
        }
    }
}