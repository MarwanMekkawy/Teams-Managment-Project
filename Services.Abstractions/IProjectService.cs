using Domain.Enums;
using Shared.ProjectDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IProjectService
    {
        Task<ProjectDto> GetByIdAsync(int id); 
        Task<ProjectDto> CreateAsync(CreateProjectDto dto); 
        Task<ProjectDto> UpdateAsync(int id, UpdateProjectDto dto);
        Task ChangeStatusAsync(int id, ProjectStatus newStatus);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task RestoreAsync(int id);

        Task<List<ProjectDto>> GetByTeamAsync(int teamId);
        Task<List<ProjectDto>> GetByOrganizationAsync( int organizationId);
    }
}
