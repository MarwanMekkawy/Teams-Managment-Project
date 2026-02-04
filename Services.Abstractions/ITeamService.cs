using Shared.Claims;
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
        Task<TeamDto> CreateAsync(CreateTeamDto dto, UserClaims userCredentials);
        Task<TeamDto> UpdateAsync(int id, UpdateTeamDto dto, UserClaims userCredentials);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id, UserClaims userCredentials);
        Task RestoreAsync(int id, UserClaims userCredentials);
        Task<List<TeamDto>> GetAllDeletedTeamsAsync(int pageNumber, int pageSize);

        Task<List<TeamDto>> GetTeamsByOrganizationAsync(int organizationId, UserClaims userCredentials,int pageNumber, int pageSize);      
        Task<List<TeamDto>> GetTeamsByUserAsync(int userId, UserClaims userCredentials, int pageNumber, int pageSize);
        Task<List<TeamDto>> GetTeamsByUserCredentialsAsync(UserClaims userCredentials, int pageNumber, int pageSize);
    }
}
