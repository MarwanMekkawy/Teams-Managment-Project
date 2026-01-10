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
        Task<List<TeamDto>> GetAllAsync();
        Task<TeamDto> GetByIdAsync(int id);
        Task<TeamDto> CreateAsync(CreateTeamDto dto);
        Task<TeamDto> UpdateAsync(int id, UpdateTeamDto dto);
        Task DeleteAsync(int id);

        Task<List<TeamDto>> GetTeamsByOrganizationAsync(int organizationId);
        Task<List<TeamMemberDto>> GetTeamMembersAsync(int teamId);
    }

}
