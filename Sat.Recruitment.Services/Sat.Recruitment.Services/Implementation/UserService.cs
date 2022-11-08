using AutoMapper;
using Sat.Recruitment.Core.Commands.CreateUser;
using Sat.Recruitment.Core.Commons;
using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Interfaces;
using Sat.Recruitment.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Services.Services
{
    public class UserService : IUserService
    {
        public readonly IUserRepository _userRepository;
        public readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> ExistsUser(User user)
        {
            var users = await _userRepository.GetUsers();
            return users.Any(u => (u.Email == user.Email || u.Phone == user.Phone) || (u.Name == user.Name && u.Address == user.Address));
        }

        public async Task<Result> CreateUser(User user)
        {
            try
            {
                if (await ExistsUser(user))
                {
                    return new Result<CreateUserRequest> { IsSuccess = false, Message = "User is duplicated" };
                }

                var usertype = user.GetUserType();
                usertype.CalculateMoney();
                await _userRepository.Add(usertype);

                var userRequest = _mapper.Map<User, CreateUserRequest>(usertype);
                return new Result<CreateUserRequest> { IsSuccess = true, Data = userRequest, Message = "User Created" };
            }
            catch (Exception)
            {
                return new Result { IsSuccess = false, Message = "Internal Server Error" };
            }
        }
    }
}
