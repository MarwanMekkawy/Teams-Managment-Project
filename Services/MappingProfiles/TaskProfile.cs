using AutoMapper;
using Shared.TaskDTOs;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<Domain.Entities.TaskEntity, TaskDto>()
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
            .ForMember(dest => dest.AssigneeEmail, opt => opt.MapFrom(src => src.Assignee.Email));

        CreateMap<CreateTaskDto, Domain.Entities.TaskEntity>();
        CreateMap<UpdateTaskDto, Domain.Entities.TaskEntity>();
    }
}
