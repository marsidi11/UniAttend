using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniAttend.Application.Auth.Queries.GetUserProfile;

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
            var query = new GetUserProfileQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}