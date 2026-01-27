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
        // Project related //
        // get tasks count in a project
        Task<int> CountOpenTasksByProjectAsync(int projectId);
        // get list of tasks in project by project id with optional task status filter
        Task<IEnumerable<TaskEntity>> GetByProjectAndStatusAsync(int projectId, TaskEntityStatus? status = null, bool tracked = false);

        // Global //
        // get overdue tasks by org id
        Task<IEnumerable<TaskEntity>> GetOverdueAsync(int organizationId);
        // get task including project and team
        Task<TaskEntity?> GetByIdWithProjectAndTeamAndMembersAsync(int taskid);
        // gets all soft deleted by org id with proj and teams
        Task<List<TaskEntity>> GetAllSoftDeletedByOrganizationIncludingProjectsAndTeamsAsync(int organizationId, int pageNumber, int pageSize, bool tracked = false);
    }
}





