using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Enums;
using Services.Abstractions;
using Shared.TaskDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<TaskDto> GetByIdAsync(int id)
        {
            var task = await unitOfWork.tasks.GetAsync(id);
            if (task == null) throw new KeyNotFoundException($"Task with ID {id} not found");
            return mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
        {
            var task = mapper.Map<TaskEntity>(dto);
            unitOfWork.tasks.Add(task);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> UpdateAsync(int id, UpdateTaskDto dto)
        {
            var task = await unitOfWork.tasks.GetAsync(id);
            if (task == null) throw new KeyNotFoundException($"Task with ID {id} not found");
            task.Title = dto.Title ?? task.Title;
            task.Description = dto.Description ?? task.Description;
            task.DueDate = dto.DueDate ?? task.DueDate;
            task.Status = dto.Status ?? task.Status;
            task.AssigneeId = dto.AssigneeId ?? task.AssigneeId;
            unitOfWork.tasks.Update(task);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<TaskDto>(task);
        }

        public async Task DeleteAsync(int id)
        {
            var task = await unitOfWork.tasks.GetAsync(id);
            if (task == null) throw new KeyNotFoundException($"Task with ID {id} not found");
            unitOfWork.tasks.Delete(task);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var task = await unitOfWork.tasks.GetAsync(id);
            if (task == null) throw new KeyNotFoundException($"Task with ID {id} not found");
            task.IsDeleted = true;
            unitOfWork.tasks.Update(task);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task RestoreAsync(int id)
        {
            var task = await unitOfWork.tasks.GetIncludingDeletedAsync(id);
            if (task == null) throw new KeyNotFoundException($"Task with ID {id} not found");
            if (!task.IsDeleted) throw new InvalidOperationException("Not deleted Entity");

            task.IsDeleted = false;
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<TaskDto>> GetTasksByProjectAsync(int projectId)
        {
            var tasks = await unitOfWork.tasks.GetByProjectAndStatusAsync(projectId);
            if (tasks == null) throw new KeyNotFoundException($"Project with ID {projectId} not found");
            return mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<List<TaskDto>> GetTasksByUserAsync(int userId)
        {
            var tasks = await unitOfWork.tasks.GetByAssigneeAndStatusAsync(userId);
            if (tasks == null) throw new KeyNotFoundException($"User with ID {userId} not found");
            return mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<List<TaskDto>> GetOverdueTasksAsync(int organizationId)
        {
            var overDueTasks = await unitOfWork.tasks.GetOverdueAsync(organizationId);
            if (overDueTasks == null) throw new KeyNotFoundException($"Organization with ID {organizationId} not found");
            return mapper.Map<List<TaskDto>>(overDueTasks);
        }

        public async Task AssignToUserAsync(int taskId, int userId)
        {
            var task = await unitOfWork.tasks.GetAsync(taskId);
            if (task == null) throw new KeyNotFoundException($"Task with ID {taskId} not found");
            var userExists  = await unitOfWork.users.ExistsAsync(userId);
            if (!userExists) throw new KeyNotFoundException($"User with ID {userId} not found");
            task.AssigneeId = userId;
            unitOfWork.tasks.Update(task);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(int id, TaskEntityStatus? status)
        {
            var task = await unitOfWork.tasks.GetAsync(id);
            if (task == null) throw new KeyNotFoundException($"Task with ID {id} not found");
            task.Status = status ?? task.Status;
            unitOfWork.tasks.Update(task);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
