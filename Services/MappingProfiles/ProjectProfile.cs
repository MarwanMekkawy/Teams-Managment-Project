using AutoMapper;
using Domain.Entities;
using Shared.ProjectDTOs;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();
        CreateMap<Project, ProjectDto>();
    }
}
