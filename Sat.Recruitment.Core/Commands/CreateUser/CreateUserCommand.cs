using MediatR;
using Sat.Recruitment.Core.Commons;

namespace Sat.Recruitment.Core.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Result>
    {
        public CreateUserRequest CreateUserRequest { get; set; }
    }
}
