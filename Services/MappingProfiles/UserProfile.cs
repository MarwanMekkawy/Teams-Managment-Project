using AutoMapper;
using Domain.Entities;
using Shared.UserDTOs;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Organization.Name))
            .ForMember(dest => dest.TeamNames, opt => opt.MapFrom(src => src.TeamMemberships.Select(tm => tm.Team.Name)));

        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();
    }
}
