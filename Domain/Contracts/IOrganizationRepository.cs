using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IOrganizationRepository : IGenericRepository<Organization,int>
    {
        // Get organization status as tuble
        Task<(int totalUsers, int totalTeams, int activeProjects, int archivedProjects, int totalTasks, 
              int completedTasks, int overdueTasks)> GetOrganizationStatsAsync(int organizationId);
    }
}
