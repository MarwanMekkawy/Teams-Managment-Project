using Domain.Enums;
using Shared.Claims;
using Shared.OrganizationDTOs;
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
        Task<ProjectDto> GetByIdAsync(int id, UserClaims userCredentials); 
        Task<ProjectDto> CreateAsync(CreateProjectDto dto, UserClaims userCredentials); 
        Task<ProjectDto> UpdateAsync(int id, UpdateProjectDto dto, UserClaims userCredentials);
        Task ChangeStatusAsync(int id, ProjectStatus newStatus, UserClaims userCredentials);
        Task DeleteAsync(int id, UserClaims userCredentials);
        Task SoftDeleteAsync(int id, UserClaims userCredentials);
        Task RestoreAsync(int id, UserClaims userCredentials);
        Task<List<ProjectDto>> GetAllDeletedProjectsAsync(int pageNumber, int pageSize);

        Task<List<ProjectDto>> GetByTeamAsync(int teamId, UserClaims userCredentials);
        Task<List<ProjectDto>> GetByOrganizationAsync( int organizationId, UserClaims userCredentials);
    }
}
