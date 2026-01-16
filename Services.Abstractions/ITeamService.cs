using Shared.OrganizationDTOs;
using Shared.TeamDTOs;
using Shared.TeamMemberDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface ITeamService
    {
        Task<TeamDto> GetByIdAsync(int id);
        Task<TeamDto> CreateAsync(CreateTeamDto dto);
        Task<TeamDto> UpdateAsync(int id, UpdateTeamDto dto);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task RestoreAsync(int id);
        Task<List<TeamDto>> GetAllDeletedTeamsAsync();

        Task<List<TeamDto>> GetTeamsByOrganizationAsync(int organizationId);      
        Task<List<TeamDto>> GetTeamsByUserAsync(int userId);
    }
}
