using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using UniAttend.Application.Features.Students.Commands.RegisterStudent;
using UniAttend.Application.Features.Schedule.Commands.CreateSchedule;

namespace UniAttend.API.Controllers
{
    [ApiController]
    public class SecretaryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SecretaryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("students")]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("schedule")]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}