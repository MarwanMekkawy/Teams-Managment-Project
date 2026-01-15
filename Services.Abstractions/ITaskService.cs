using Domain.Enums;
using Shared.TaskDTOs;


namespace Services.Abstractions
{
    public interface ITaskService
    {
        Task<TaskDto> GetByIdAsync(int id);
        Task<TaskDto> CreateAsync(CreateTaskDto dto);
        Task<TaskDto> UpdateAsync(int id, UpdateTaskDto dto);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task RestoreAsync(int id);

        Task<List<TaskDto>> GetTasksByProjectAsync(int projectId);
        Task<List<TaskDto>> GetTasksByUserAsync(int userId);
        Task<List<TaskDto>> GetOverdueTasksAsync(int organizationId);

        Task ChangeStatusAsync(int id, TaskEntityStatus status);
        Task AssignToUserAsync(int taskId, int userId);
    }
}
