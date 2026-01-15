using AutoMapper;
using Domain.Entities;
using Shared.TeamDTOs;

public class TeamProfile : Profile
{
    public TeamProfile()
    {
        CreateMap<Team, TeamDto>();
        CreateMap<CreateTeamDto, Team>();
        CreateMap<UpdateTeamDto, Team>();
    }
}
