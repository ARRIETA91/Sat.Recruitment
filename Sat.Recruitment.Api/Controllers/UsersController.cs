using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Sat.Recruitment.Core.Commands.CreateUser;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var result = await _mediator.Send(new CreateUserCommand { CreateUserRequest = request });

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
