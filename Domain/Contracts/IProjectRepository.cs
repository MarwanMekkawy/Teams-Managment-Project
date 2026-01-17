using Domain.Entities;
using Domain.Enums;

namespace Domain.Contracts
{
    public interface IProjectRepository : IGenericRepository<Project, int>
    {
        // Get all projects of a team 
        Task<IEnumerable<Project>> GetByTeamAsync(int teamId, bool tracked = false);

        // Get projects of a team filtered by status (or all if null)
        Task<IEnumerable<Project>> GetByTeamAndStatusAsync(int teamId, ProjectStatus? status = null, bool tracked = false);

        // Get projects of an entire organization filtered by status (or all if null)
        Task<IEnumerable<Project>> GetByOrganizationAndStatusAsync(int organizationId, ProjectStatus? status = null, bool tracked = false);
    }
}
