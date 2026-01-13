using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ITeamMemberRepository 
    {
        void AddMember(TeamMember entity);
        void RemoveMember(int teamId, int userId);
        Task<bool> ExistsAsync(int teamId, int userId);
        Task<IEnumerable<int>> GetUserIdsByTeamAsync(int teamId);
        Task<IEnumerable<int>> GetTeamIdsByUserAsync(int userId);
    }
}
