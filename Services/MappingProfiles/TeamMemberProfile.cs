using AutoMapper;
using Domain.Entities;
using Shared.TeamMemberDTOs;

public class TeamMemberProfile : Profile
{
    public TeamMemberProfile()
    {
        CreateMap<TeamMember, TeamMemberDto>()
            .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.Name));

        CreateMap<CreateTeamMemberDto, TeamMember>();
    }
}
