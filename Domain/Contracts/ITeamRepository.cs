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
        // get list of teams by org id
        Task<IEnumerable<Team>> GetByOrganizationAsync(int organizationId, bool tracked = false);
        // get list of teams by user id
        Task<IEnumerable<Team>> GetByUserAsync(int userId, bool tracked = false);
        // get list of teams by user id & org id
        Task<IEnumerable<Team>> GetByUserAndOrganizationAsync(int userId, int organizationId, bool tracked = false);
    }
}
