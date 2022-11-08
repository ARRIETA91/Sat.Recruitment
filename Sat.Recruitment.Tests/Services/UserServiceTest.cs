using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace Sat.Recruitment.Test.Services
{
    [TestClass]
    public class UserServiceTest
    {
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _userService = new UserService(_userRepository.Object, _mapper.Object);
        }

        [TestMethod]
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
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull((result as Result<CreateUserRequest>).Data);
        }

        
        [TestMethod]
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
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Message, "User is duplicated");
        }

        [TestMethod]
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
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Message, "Internal Server Error");
        }


        [TestMethod]
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
            Assert.IsTrue(result);
        }

        
        [TestMethod]
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
            Assert.AreEqual(UserType.Normal, usersList[0].UserType);
            Assert.AreEqual(UserType.SuperUser, usersList[3].UserType);
            Assert.AreEqual(UserType.Premium, usersList[4].UserType);
            Assert.AreEqual(Convert.ToDecimal(123.20), usersList[0].Money);
            Assert.AreEqual(Convert.ToDecimal(162), usersList[1].Money);
            Assert.AreEqual(Convert.ToDecimal(100), usersList[2].Money);
            Assert.AreEqual(Convert.ToDecimal(240), usersList[3].Money);
            Assert.AreEqual(Convert.ToDecimal(1200), usersList[4].Money);
        }
    }
}

