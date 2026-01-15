using AutoMapper;
using Domain.Entities;
using Shared.TaskDTOs;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskEntity, TaskDto>();
        CreateMap<CreateTaskDto, TaskEntity>();
        CreateMap<UpdateTaskDto, TaskEntity>();
    }
}
