using Api.Contracts.User;
using Application.User.Commands;
using AutoMapper;

namespace Api.MappingProfiles
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<Register, RegisterUser>();
        }
       
    }
}
