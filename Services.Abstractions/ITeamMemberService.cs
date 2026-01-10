using Shared.TeamMemberDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface ITeamMemberService
    {
        Task AddMemberAsync(CreateTeamMemberDto dto);
        Task RemoveMemberAsync(int teamId, int userId);
        Task<List<TeamMemberDto>> GetMembersByTeamAsync(int teamId);
        Task<List<TeamMemberDto>> GetTeamsByUserAsync(int userId);
    }

}
