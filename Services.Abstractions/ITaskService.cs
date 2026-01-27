using Domain.Enums;
using Shared.Claims;
using Shared.OrganizationDTOs;
using Shared.TaskDTOs;


namespace Services.Abstractions
{
    public interface ITaskService
    {
        Task<TaskDto> GetByIdAsync(int id, UserClaims userCredentials);
        Task<TaskDto> CreateAsync(CreateTaskDto dto, UserClaims userCredentials);
        Task<TaskDto> UpdateAsync(int id, UpdateTaskDto dto, UserClaims userCredentials);
        Task DeleteAsync(int id, UserClaims userCredentials);
        Task SoftDeleteAsync(int id, UserClaims userCredentials);
        Task RestoreAsync(int id, UserClaims userCredentials);
        Task<List<TaskDto>> GetAllDeletedTasksAsync(int pageNumber, int pageSize, UserClaims userCredentials);
        Task<List<TaskDto>> GetOverdueTasksAsync(int organizationId, UserClaims userCredentials);

        Task ChangeStatusAsync(int id, TaskEntityStatus? status, UserClaims userCredentials);
        Task AssignToUserAsync(int taskId, int userId, UserClaims userCredentials);
    }
}
