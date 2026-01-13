using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ITaskRepository : IGenericRepository<TaskEntity, int>
    {
        // Project related
        Task<int> CountOpenTasksByProjectAsync(int projectId);
        Task<IEnumerable<TaskEntity>> GetByProjectAsync(int projectId, bool tracked = false);
        Task<IEnumerable<TaskEntity>> GetByProjectAndStatusAsync(int projectId, TaskEntityStatus? status = null, bool tracked = false);

        // User/Assignee related
        Task<IEnumerable<TaskEntity>> GetByAssigneeAsync(int userId, bool tracked = false);
        Task<IEnumerable<TaskEntity>> GetByAssigneeAndStatusAsync(int userId, TaskEntityStatus? status = null, bool tracked = false);

        // Global
        Task<IEnumerable<TaskEntity>> GetOverdueAsync(int organizationId);
    }
}




