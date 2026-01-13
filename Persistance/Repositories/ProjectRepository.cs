using Domain.Contracts;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class ProjectRepository : GenericRepository<Project, int> , IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Project>> GetByOrganizationAndStatusAsync(int organizationId, ProjectStatus? status = null, bool tracked = false)
        {
            var query =  _context.Projects.Where(p => p.Team.OrganizationId == organizationId).AsQueryable();
            if (!tracked) query = query.AsNoTracking();
            if (status != null) query = query.Where(p => p.Status == status.Value);
            return await query.ToListAsync();         
        }

        public async Task<IEnumerable<Project>> GetByTeamAndStatusAsync(int teamId, ProjectStatus? status = null, bool tracked = false)
        {
            var query = _context.Projects.Where(p => p.TeamId == teamId).AsQueryable();
            if (!tracked) query = query.AsNoTracking();
            if(status != null) query=query.Where(p=>p.Status == status.Value);
            return await query.ToListAsync() ;
        }

        public async Task<IEnumerable<Project>> GetByTeamAsync(int teamId, bool tracked = false)
        {
            var query = _context.Projects.Where(p=>p.TeamId == teamId).AsQueryable();
            if (!tracked) query = query.AsNoTracking();
            return await query.ToListAsync() ;  
        }
    }
}
