using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sat.Recruitment.Core.Commands.CreateUser;
using System.Linq;

namespace Sat.Recruitment.Test.Commands.CreateUser
{
    [TestClass]
    public class CreateUserCommandValidatorTest
    {
        private readonly CreateUserCommandValidator _validator;

        public CreateUserCommandValidatorTest()
        {
            _validator = new CreateUserCommandValidator();
        }

        [TestMethod]
        public void Handle_CreateUser_With_Name_Is_Required()
        {
            //arrange
            var command = new CreateUserCommand
            {
                CreateUserRequest = new CreateUserRequest
                {
                    Name = null,
                    Address = "Pizzurno 763",
                    Email = "vale3@gmail.com",
                    Phone = "1164874553",
                    Money = 100,
                    UserType = "Normal"
                }
            };

            //act
            var result = _validator.Validate(command);

            //assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The name is required", result.Errors.ElementAt(0).ErrorMessage);
        }

        [TestMethod]
        public void Handle_CreateUser_With_Adress_Is_Required()
        {
            //arrange
            var command = new CreateUserCommand
            {
                CreateUserRequest = new CreateUserRequest
                {
                    Name = "Valeria Arrieta",
                    Address = "",
                    Email = "vale3@gmail.com",
                    Phone = "1164874553",
                    Money = 100,
                    UserType = "Normal"
                }
            };

            //act
            var result = _validator.Validate(command);

            //assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The address is required", result.Errors.ElementAt(0).ErrorMessage);
        }

        [TestMethod]
        public void Handle_CreateUser_With_Email_Is_Required()
        {
            //arrange
            var command = new CreateUserCommand
            {
                CreateUserRequest = new CreateUserRequest
                {
                    Name = "Valeria Arrieta",
                    Address = "Pizzurno 763",
                    Email = null,
                    Phone = "1164874553",
                    Money = 100,
                    UserType = "Normal"
                }
            };

            //act
            var result = _validator.Validate(command);

            //assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The email is required", result.Errors.ElementAt(0).ErrorMessage);
        }

        [TestMethod]
        public void Handle_CreateUser_With_Email_Is_Invalid()
        {
            //arrange
            var command = new CreateUserCommand
            {
                CreateUserRequest = new CreateUserRequest
                {
                    Name = "Valeria Arrieta",
                    Address = "Pizzurno 763",
                    Email = "valegmail.com",
                    Phone = "1164874553",
                    Money = 100,
                    UserType = "Normal"
                }
            };

            //act
            var result = _validator.Validate(command);

            //assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The email is not valid", result.Errors.ElementAt(0).ErrorMessage);
        }

        [TestMethod]
        public void Handle_CreateUser_With_Phone_Is_Required()
        {
            //arrange
            var command = new CreateUserCommand
            {
                CreateUserRequest = new CreateUserRequest
                {
                    Name = "Valeria Arrieta",
                    Address = "Pizzurno 763",
                    Email = "vale3@gmail.com",
                    Phone = "",
                    Money = 100,
                    UserType = "Normal"
                }
            };

            //act
            var result = _validator.Validate(command);

            //assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The phone is required", result.Errors.ElementAt(0).ErrorMessage);
        }
    }
}
