using Api.Contracts.User;
using Application.Dto;
using Application.User.Commands;
using AutoMapper;

namespace Api.MappingProfiles
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<Register, RegisterUser>();
            CreateMap<Login, LoginUser>();
            CreateMap<LogedInUserDto, User>();
        }
       
    }
}
