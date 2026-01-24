using Shared.OrganizationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IOrganizationService
    {
        Task<(int totalUsers, int totalTeams, int activeProjects, int archivedProjects, int totalTasks, int completedTasks, int overdueTasks)>GetStatsAsync(int Id);
        Task<List<string>> GetAllAsync(int pageNumber, int pageSize);
        Task<OrganizationDto> GetByIdAsync(int id);
        Task<OrganizationDto> CreateAsync(CreateOrganizationDto dto);
        Task<OrganizationDto> UpdateAsync(int id, UpdateOrganizationDto dto);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task RestoreAsync(int id);
        Task<List<OrganizationDto>> GetAllDeletedOrganizationsAsync();
    }
}

