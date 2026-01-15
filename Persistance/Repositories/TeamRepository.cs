using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class TeamRepository : GenericRepository<Team , int> , ITeamRepository
    {
        public TeamRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Team>> GetByOrganizationAsync(int organizationId, bool tracked = false)
        {
            var query = _context.Teams.Where(t=>t.OrganizationId == organizationId);
            if (tracked) query = query.AsNoTracking();
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Team>> GetByUserAsync(int userId, bool tracked = false)
        {
            var query = _context.Teams.Where(t => t.Members.Any(m=>m.UserId == userId));
            if (tracked) query = query.AsNoTracking();
            return await query.ToListAsync();
        }
    }
}
