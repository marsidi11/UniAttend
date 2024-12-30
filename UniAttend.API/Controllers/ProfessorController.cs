using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using UniAttend.Application.Features.Attendance.Commands.ConfirmAttendance;

namespace UniAttend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireProfessorRole")]
    public class ProfessorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfessorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("classes/{classId}/confirm")]
        public async Task<IActionResult> ConfirmAttendance(int classId)
        {
            var command = new ConfirmAttendanceCommand { ClassId = classId };
            await _mediator.Send(command);
            return Ok();
        }
    }
}