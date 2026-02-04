using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ITeamRepository : IGenericRepository<Team, int>
    {
        // get list of teams by org id [paginated]
        Task<IEnumerable<Team>> GetByOrganizationAsync(int organizationId, int pageNumber, int pageSize, bool tracked = false);
        // get list of teams by user id [paginated]
        Task<IEnumerable<Team>> GetByUserAsync(int userId, int pageNumber, int pageSize, bool tracked = false);
        // get list of teams by user id & org id [paginated]
        Task<IEnumerable<Team>> GetByUserAndOrganizationAsync(int userId, int organizationId, int pageNumber, int pageSize, bool tracked = false);
        // checks if team exists in org
        Task<bool> IsInOrganization(int teamId, int organizationId);
    }
}
