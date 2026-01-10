using AutoMapper;
using Domain.Entities;
using Shared.ProjectDTOs;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.Name))
            .ForMember(dest => dest.TaskTitles, opt => opt.MapFrom(src => src.Tasks.Select(t => t.Title)));

        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();
    }
}
