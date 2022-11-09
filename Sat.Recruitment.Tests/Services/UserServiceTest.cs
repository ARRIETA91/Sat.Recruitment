using AutoMapper;
using Moq;
using Sat.Recruitment.Core.Commands.CreateUser;
using Sat.Recruitment.Core.Commons;
using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Enums;
using Sat.Recruitment.Repositories.Interfaces;
using Sat.Recruitment.Services.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sat.Recruitment.Test.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _userService = new UserService(_userRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task UserService_CreateUser_Return_Success()
        {
            //arrange
            var user = new User
            {
                Name = "Valeria Arrieta",
                Address = "Pizzurno 763",
                Email = "vale3@gmail.com",
                Phone = "1164874553",
                Money = 100,
                UserType = UserType.Normal
            };

            var userRequest = new CreateUserRequest
            {
                Name = "Valeria Arrieta",
                Address = "Pizzurno 763",
                Email = "vale3@gmail.com",
                Phone = "1164874553",
                Money = 100,
                UserType = "Normal"
            };

            _userRepository.Setup(x => x.GetUsers()).Returns(Task.FromResult<IEnumerable<User>>(new List<User>() { new User() }));
            _userRepository.Setup(x => x.Add(It.IsAny<User>()));

            _mapper.Setup(x => x.Map<User, CreateUserRequest>(It.IsAny<User>())).Returns(userRequest);

            //act
            var result = await _userService.CreateUser(user);

            //assert
            Assert.True(result.IsSuccess);
            Assert.NotNull((result as Result<CreateUserRequest>).Data);
        }

        [Fact]
        public async Task UserService_CreateUser_When_AlreadyExists_Return_No_Success()
        {
            //arrange
            var user = new SuperUser
            {
                Name = "Valeria Arrieta",
                Address = "Pizzurno 763",
                Email = "vale3@gmail.com",
                Phone = "1164874553",
                Money = 100,
                UserType = UserType.SuperUser
            };
            _userRepository.Setup(x => x.GetUsers()).Returns(Task.FromResult<IEnumerable<User>>(new List<User>() { user }));

            //act
            var result = await _userService.CreateUser(user);

            //assert
            Assert.False(result.IsSuccess);
            Assert.Equal("User is duplicated", result.Message);
        }

        [Fact]
        public async Task UserService_CreateUser_Return_InternalServerError()
        {
            //arrange
            var user = new SuperUser
            {
                Name = "Valeria Arrieta",
                Address = "Pizzurno 763",
                Email = "vale3@gmail.com",
                Phone = "1164874553",
                Money = 100,
                UserType = UserType.SuperUser
            };
            _userRepository.Setup(x => x.GetUsers()).Returns(Task.FromResult<IEnumerable<User>>(null));

            //act
            var result = await _userService.CreateUser(user);

            //assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Internal Server Error", result.Message);
        }

        [Fact]
        public async Task UserServices_UserExists_Return_True()
        {
            //arrange
            var user = new User
            {
                Name = "Valeria Arrieta",
                Address = "Pizzurno 763",
                Email = "vale3@gmail.com",
                Phone = "1164874553",
                Money = 100,
                UserType = UserType.Normal
            };
            _userRepository.Setup(x => x.GetUsers()).Returns(Task.FromResult<IEnumerable<User>>(new List<User>() { user }));

            //act
            var result = await _userService.ExistsUser(user);

            //assert
            Assert.True(result);
        }

        [Fact]
        public void UserService_CalculateMoney_By_UserType()
        {
            //init
            var userNormal_1 = new User
            {
                Money = 110,//expect 123.2
                UserType = UserType.Normal
            };
            var userNormal_2 = new User
            {
                Money = 90,//expect 97.2
                UserType = UserType.Normal
            };
            var userNormal_3 = new User
            {
                Money = 100,//expect 100
                UserType = UserType.Normal
            };
            var userSuper = new User
            {
                Money = 200,//expect 240
                UserType = UserType.SuperUser
            };
            var userPremium = new User
            {
                Money = 400,//expect 1200
                UserType = UserType.Premium
            };

            var list = new List<User>() { userNormal_1, userNormal_2, userNormal_3, userSuper, userPremium };
            var usersList = new List<User>();

            //act
            list.ForEach(x =>
            {
                var newUser = x.GetUserType();
                newUser.CalculateMoney();
                usersList.Add(newUser);
            });

            //assert
            Assert.Equal(UserType.Normal, usersList[0].UserType);
            Assert.Equal(UserType.SuperUser, usersList[3].UserType);
            Assert.Equal(UserType.Premium, usersList[4].UserType);
            Assert.Equal(Convert.ToDecimal(123.20), usersList[0].Money);
            Assert.Equal(Convert.ToDecimal(162), usersList[1].Money);
            Assert.Equal(Convert.ToDecimal(100), usersList[2].Money);
            Assert.Equal(Convert.ToDecimal(240), usersList[3].Money);
            Assert.Equal(Convert.ToDecimal(1200), usersList[4].Money);
        }
    }
}

