using Application.Dto;
using AutoMapper;
using Domain.Aggregates.UserProfileAggregate;

namespace Application.MappingProfiles
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<UserProfile, LogedInUserDto>()
                .ForMember(
                    dest => dest.EmailAddress,
                    opt => opt.MapFrom(src => src.BasicInfo.EmailAddress)
                )
                .ForMember(
                    dest => dest.FirstName,
                    opt => opt.MapFrom(src => src.BasicInfo.FirstName)
                )
                .ForMember(
                    dest => dest.LastName,
                    opt => opt.MapFrom(src => src.BasicInfo.LastName)
                );
        }
    }
}