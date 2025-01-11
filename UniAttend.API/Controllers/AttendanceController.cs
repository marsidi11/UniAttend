using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using UniAttend.Application.Features.Attendance.Commands.ConfirmAttendance;
using UniAttend.Application.Features.Attendance.Commands.RecordCardAttendance;
using UniAttend.Application.Features.Attendance.Commands.RecordOtpAttendance;
using UniAttend.Application.Features.Attendance.Queries;
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
    }
}