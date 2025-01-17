using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniAttend.Application.Features.Attendance.Commands.RecordOtpAttendance;
using UniAttend.Application.Features.Attendance.DTOs;
using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Interfaces.Services;
using System.Security.Claims;

namespace UniAttend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/attendance/otp")]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _otpService;
        private readonly IMediator _mediator;

        public OtpController(IOtpService otpService, IMediator mediator)
        {
            _otpService = otpService;
            _mediator = mediator;
        }

        [HttpPost("generate")]
        [Authorize(Roles = "Professor")]
        public async Task<ActionResult<OtpCode>> GenerateOtp([FromBody] GenerateOtpRequest request, CancellationToken cancellationToken)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var otpCode = await _otpService.GenerateOtpAsync(
                request.ClassId,
                request.StudentId,
                cancellationToken);

            return Ok(otpCode);
        }

        [HttpPost("validate")]
        public async Task<ActionResult<OtpValidationResponse>> ValidateOtp([FromBody] ValidateOtpRequest request, CancellationToken cancellationToken)
        {
            var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            
            var isValid = await _otpService.ValidateOtpAsync(
                request.Code,
                request.ClassId,
                studentId,
                cancellationToken);

            if (!isValid)
            {
                return BadRequest(new OtpValidationResponse 
                { 
                    IsValid = false,
                    Message = "Invalid or expired OTP code"
                });
            }

            var command = new RecordOtpAttendanceCommand
            {
                OtpCode = request.Code,
                ClassId = request.ClassId,
                StudentId = studentId
            };

            var attendanceRecord = await _mediator.Send(command, cancellationToken);

            return Ok(new OtpValidationResponse
            {
                IsValid = true,
                Message = "Attendance recorded successfully",
                AttendanceRecord = attendanceRecord
            });
        }

        [HttpGet("{classId}/current")]
        [Authorize(Roles = "Professor")]
        public async Task<ActionResult<OtpCode>> GetCurrentOtp(int classId, CancellationToken cancellationToken)
        {
            var otpCode = await _otpService.GetCurrentOtpAsync(classId, cancellationToken);
            if (otpCode == null)
            {
                return NotFound();
            }

            return Ok(otpCode);
        }
    }

    public class GenerateOtpRequest
    {
        public int ClassId { get; set; }
        public int StudentId { get; set; }
    }

    public class ValidateOtpRequest
    {
        public string Code { get; set; } = string.Empty;
        public int ClassId { get; set; }
    }
}