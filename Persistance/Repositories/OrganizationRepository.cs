using Domain.Contracts;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace Persistance.Repositories
{
    public class OrganizationRepository : GenericRepository<Organization,int> ,IOrganizationRepository
    {
        public OrganizationRepository(AppDbContext context):base(context) {}

        public async Task<(int totalUsers, int totalTeams, int activeProjects, int archivedProjects, int totalTasks, int completedTasks, int overdueTasks)?>
        GetOrganizationStatsAsync(int organizationId)
        {          
            var stats = await _context.Organizations
                .Where(o => o.Id == organizationId)
                .Select(o => new{
                    TotalUsers = o.Users.Count(),
                    TotalTeams = o.Teams.Count(),
                    ActiveProjects = o.Teams.SelectMany(t => t.Projects).Count(p => p.Status == ProjectStatus.Active),
                    ArchivedProjects = o.Teams.SelectMany(t => t.Projects).Count(p => p.Status == ProjectStatus.Archived),
                    TotalTasks = o.Teams.SelectMany(t => t.Projects).SelectMany(p => p.Tasks).Count(),
                    CompletedTasks = o.Teams.SelectMany(t => t.Projects).SelectMany(p => p.Tasks) .Count(t => t.Status == TaskEntityStatus.Done),
                    OverdueTasks = o.Teams.SelectMany(t => t.Projects).SelectMany(p => p.Tasks)
                                   .Count(t => t.Status != TaskEntityStatus.Done && t.DueDate.HasValue && t.DueDate.Value < DateTime.Now),
                }).FirstOrDefaultAsync();

            if (stats == null) return null;
            return (stats.TotalUsers, stats.TotalTeams, stats.ActiveProjects, stats.ArchivedProjects, stats.TotalTasks, stats.CompletedTasks, stats.OverdueTasks);
        }
        public async Task<bool> IsOrgNameExistsAsync(string orgName)
        {
            return await _context.Organizations.AnyAsync(o => o.Name == orgName);
        }
    }
}
