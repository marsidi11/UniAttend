using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using UniAttend.Application.Features.Users.DTOs;
using UniAttend.Application.Features.Users.Commands.CreateUser;
using UniAttend.Application.Features.Users.Commands.ChangePassword;
using UniAttend.Application.Features.Users.Commands.UpdateProfile;
using UniAttend.Application.Features.Users.Commands.UpdateUser;
using UniAttend.Application.Features.Users.Commands.DeactivateUser;
using UniAttend.Application.Features.Users.Queries.GetUsers;
using UniAttend.Application.Features.Users.Queries.GetUserDetails;
using UniAttend.Application.Features.Users.Queries.GetUserProfile;
using UniAttend.API.Extensions;
using UniAttend.Core.Enums;

namespace UniAttend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll(
            [FromQuery] UserRole? role,
            [FromQuery] bool? isActive,
            CancellationToken cancellationToken)
        {
            var query = new GetUsersQuery { Role = role, IsActive = isActive };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<ActionResult<UserDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var query = new GetUsersQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);
            var user = result.FirstOrDefault();
            
            if (user == null)
                return NotFound();
                
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> Create(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result }, result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateUserCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest();

            // Only allow users to update their own profile unless they're an admin
            if (id != User.GetUserId() && !User.IsInRole("Admin"))
                return Forbid();

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deactivate(int id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeactivateUserCommand { Id = id }, cancellationToken);
            return NoContent();
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileDto>> GetProfile(CancellationToken cancellationToken)
        {
            var query = new GetUserProfileQuery { UserId = User.GetUserId() };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileCommand command, CancellationToken cancellationToken)
        {
            command.UserId = User.GetUserId();
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpGet("details/{id}")]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<ActionResult<UserDetailsDto>> GetUserDetails(int id, CancellationToken cancellationToken)
        {
            var query = new GetUserDetailsQuery { UserId = id };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("details")]
        public async Task<ActionResult<UserDetailsDto>> GetCurrentUserDetails(CancellationToken cancellationToken)
        {
            var query = new GetUserDetailsQuery { UserId = User.GetUserId() };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            command.UserId = User.GetUserId();
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}