using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniAttend.Application.Auth.Commands.Login;
using UniAttend.Application.Auth.Commands.Register;

/// <summary>
/// Controller responsible for authentication operations including user registration and login.
/// </summary>
namespace UniAttend.API.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}