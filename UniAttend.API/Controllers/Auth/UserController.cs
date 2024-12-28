using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using UniAttend.Application.Features.Users.Commands.UpdateProfile;
using UniAttend.Application.Features.Users.Commands.ChangePassword;
using UniAttend.Application.Features.Users.Queries.GetUserDetails;
using UniAttend.Application.Features.Users.Queries.GetUserProfile;
using UniAttend.API.Extensions;

namespace UniAttend.API.Controllers.Auth
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var query = new GetUserProfileQuery { UserId = User.GetUserId() };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileCommand command)
        {
            command.UserId = User.GetUserId();
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            command.UserId = User.GetUserId();
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetUserDetails()
        {
            var query = new GetUserDetailsQuery { UserId = User.GetUserId() };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}