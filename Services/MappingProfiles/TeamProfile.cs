using AutoMapper;
using Domain.Entities;
using Shared.TeamDTOs;

public class TeamProfile : Profile
{
    public TeamProfile()
    {
        CreateMap<Team, TeamDto>()
            .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Organization.Name))
            .ForMember(dest => dest.MemberNames, opt => opt.MapFrom(src => src.Members.Select(m => m.User.Name)))
            .ForMember(dest => dest.ProjectNames, opt => opt.MapFrom(src => src.Projects.Select(p => p.Name)));

        CreateMap<CreateTeamDto, Team>();
        CreateMap<UpdateTeamDto, Team>();
    }
}
