using Shared.Claims;
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
        Task AddMemberAsync(CreateTeamMemberDto dto, UserClaims userCredentials);
        Task RemoveMemberAsync(int teamId, int userId, UserClaims userCredentials);
        Task<bool> IsMemberAsync(int teamId, int userId, UserClaims userCredentials);
    }
}

