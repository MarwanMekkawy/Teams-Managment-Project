using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class TeamRepository : GenericRepository<Team , int> , ITeamRepository
    {
        public TeamRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Team>> GetByOrganizationAsync(int organizationId, int pageNumber, int pageSize, bool tracked = false)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.Teams.Where(t=>t.OrganizationId == organizationId);
            if (!tracked) query = query.AsNoTracking();
            return await query.OrderBy(t => t.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public async Task<IEnumerable<Team>> GetByUserAsync(int userId, int pageNumber, int pageSize, bool tracked = false)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.Teams.Where(t => t.Members.Any(m=>m.UserId == userId));
            if (!tracked) query = query.AsNoTracking();
            return await query.OrderBy(t => t.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public async Task<IEnumerable<Team>> GetByUserAndOrganizationAsync(int userId, int organizationId, int pageNumber, int pageSize, bool tracked = false)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            var query = _context.Teams.Where(t => t.Members.Any(m => m.UserId == userId) && t.OrganizationId == organizationId);
            if (!tracked) query = query.AsNoTracking();
            return await query.OrderBy(t => t.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public async Task<bool> IsInOrganization (int teamId, int organizationId)
        {
           return  await _context.Teams.AnyAsync(t=>t.Id==teamId && t.OrganizationId==organizationId);
        }
    }
}
