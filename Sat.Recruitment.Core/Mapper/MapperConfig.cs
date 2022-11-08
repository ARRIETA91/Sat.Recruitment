using AutoMapper;
using Sat.Recruitment.Core.Commands.CreateUser;
using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Enums;
using System;

namespace Sat.Recruitment.Api.Mapper
{
    public static class MapperConfig
    {
        public static IMapper GetMappingConfig()
        {
            var mappingConfig = new MapperConfiguration(_ =>
            {
                _.AddProfile(new UserProfile());
            });

            return mappingConfig.CreateMapper();
        }
    }

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserRequest, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLower().Trim()))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => (UserType)Enum.Parse(typeof(UserType), src.UserType)));

            CreateMap<User, CreateUserRequest>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLower().Trim()))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType));
        }
    }
}
