using AutoMapper;
using MediatR;
using Sat.Recruitment.Core.Commons;
using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Sat.Recruitment.Core.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<CreateUserRequest, User>(command.CreateUserRequest);

            var result = await _userService.CreateUser(user);

            return result;
        }

    }
}
