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
    public class TaskRepository : GenericRepository<TaskEntity,int> , ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context) { }

        public async Task<int> CountOpenTasksByProjectAsync(int projectId)
        {
            return await _context.Tasks.Where(t => t.ProjectId == projectId && (t.Status == TaskEntityStatus.Todo || t.Status == TaskEntityStatus.InProgress)).CountAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetByProjectAndStatusAsync(int projectId, TaskEntityStatus? status = null, bool tracked = false)
        {
            var query = _context.Tasks.Where(t => t.ProjectId == projectId).AsQueryable();
            if (!tracked) query = query.AsNoTracking();
            if (status != null ) query = query.Where(t=>t.Status == status.Value);
            return await query.ToListAsync() ;
        }

        public async Task<IEnumerable<TaskEntity>> GetByAssigneeAndStatusAsync(int userId, TaskEntityStatus? status = null, bool tracked = false)
        {
            var query = _context.Tasks.Where(t => t.AssigneeId == userId).AsQueryable();
            if (!tracked) query = query.AsNoTracking();
            if(status != null) query = query.Where(t=> t.Status == status.Value);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetOverdueAsync(int organizationId)
        {
            return await _context.Tasks
                .Where(t => t.Assignee.OrganizationId == organizationId && (t.Status == TaskEntityStatus.Todo || t.Status == TaskEntityStatus.InProgress)
                         && t.DueDate.HasValue && t.DueDate.Value < DateTime.Now).ToListAsync();
        }

        public async Task<TaskEntity?> GetByIdWithProjectAndTeamAndMembersAsync(int taskId)
        {
            return await _context.Tasks.AsNoTracking().Include(t => t.Project).ThenInclude(p => p.Team)
                                       .ThenInclude(t => t.Members).FirstOrDefaultAsync(t => t.Id == taskId);
        }
    }
}
