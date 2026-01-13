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
        Task<IEnumerable<Team>> GetByOrganizationAsync(int organizationId, bool tracked = false);
    }
}
