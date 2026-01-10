using Shared.TaskDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllAsync();
        Task<TaskDto> GetByIdAsync(int id);
        Task<TaskDto> CreateAsync(CreateTaskDto dto);
        Task<TaskDto> UpdateAsync(int id, UpdateTaskDto dto);
        Task DeleteAsync(int id);

        Task<List<TaskDto>> GetTasksByProjectAsync(int projectId);
        Task<List<TaskDto>> GetTasksByUserAsync(int userId);
    }

}
