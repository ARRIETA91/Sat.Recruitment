using AutoMapper;
using Moq;
using Sat.Recruitment.Core.Commands.CreateUser;
using Sat.Recruitment.Core.Commons;
using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Enums;
using Sat.Recruitment.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Sat.Recruitment.Test.Commands.CreateUser
{
    public class CreateUserCommandHandlerTest
    {
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly CreateUserCommandHandler _handler;

        public CreateUserCommandHandlerTest()
        {
            _handler = new CreateUserCommandHandler(_userService.Object, _mapper.Object);
        }

        [Fact]
        public async Task Handle_CreateUser_Return_Success()
        {
            //arrange
            var command = new CreateUserCommand
            {
                CreateUserRequest = new CreateUserRequest
                {
                    Name = "Valeria Arrieta",
                    Address = "Pizzurno 763",
                    Email = "vale3@gmail.com",
                    Phone = "1164874553",
                    Money = 100,
                    UserType = "Normal"
                }
            };

            var user = new User
            {
                Name = "Valeria Arrieta",
                Address = "Pizzurno 763",
                Email = "vale3@gmail.com",
                Phone = "1164874553",
                Money = 100,
                UserType = UserType.Normal
            };

            _mapper.Setup(x => x.Map<CreateUserRequest, User>(command.CreateUserRequest)).Returns(user);
            _userService.Setup(x => x.CreateUser(user)).Returns(Task.FromResult(new Result { IsSuccess = true, Message = "User Created" }));

            //act
            var result = await _handler.Handle(command, CancellationToken.None);

            //assert
            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Message);
        }

        [Fact]
        public async Task Handle_CreateUser_Return_No_Success()
        {
            //arrange
            var command = new CreateUserCommand
            {
                CreateUserRequest = new CreateUserRequest
                {
                    Name = "Valeria Arrieta",
                    Address = "Pizzurno 763",
                    Email = "vale3@gmail.com",
                    Phone = "1164874553",
                    Money = 100,
                    UserType = "Normal"
                }
            };

            var user = new User
            {
                Name = "Valeria Arrieta",
                Address = "Pizzurno 763",
                Email = "vale3@gmail.com",
                Phone = "1164874553",
                Money = 100,
                UserType = UserType.Normal
            };

            _mapper.Setup(x => x.Map<CreateUserRequest, User>(command.CreateUserRequest)).Returns(user);
            _userService.Setup(x => x.CreateUser(user)).Returns(Task.FromResult(new Result { IsSuccess = false, Message = "User is duplicated" }));

            //act
            var result = await _handler.Handle(command, CancellationToken.None);

            //assert
            Assert.False(result.IsSuccess);
            Assert.Equal("User is duplicated", result.Message);
        }
    }
}
