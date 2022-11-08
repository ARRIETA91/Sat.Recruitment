using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Sat.Recruitment.Core.Commands.CreateUser;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator,ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            _logger.LogInformation($"CreateUserRequest - {JsonConvert.SerializeObject(request)}");
            var result = await _mediator.Send(new CreateUserCommand { CreateUserRequest = request });

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
