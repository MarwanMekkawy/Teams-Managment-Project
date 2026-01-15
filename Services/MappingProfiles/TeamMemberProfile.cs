using AutoMapper;
using Domain.Entities;
using Shared.TeamMemberDTOs;

public class TeamMemberProfile : Profile
{
    public TeamMemberProfile()
    {
        CreateMap<TeamMember, TeamMemberDto>();
        CreateMap<CreateTeamMemberDto, TeamMember>();
    }
}
