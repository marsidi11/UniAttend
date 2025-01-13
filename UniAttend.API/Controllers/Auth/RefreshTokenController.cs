using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniAttend.Application.Features.Auth.Commands.RefreshToken;

namespace UniAttend.API.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefreshTokenController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RefreshTokenController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}