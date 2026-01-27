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
        // gets team membership entity
        Task<TeamMember> GetByTeamAndUserAsync(int teamId, int userId);
        // adds entity teamid & userid to the table
        void AddMember(TeamMember entity);
        // removes entity teamid & userid from the table
        void RemoveMember(TeamMember entity);
        // gets list of users ids in a certain team with team id
        Task<IEnumerable<int>> GetUserIdsByTeamAsync(int teamId);
        // gets list of teams ids with certain users with user id
        Task<IEnumerable<int>> GetTeamIdsByUserAsync(int userId);
        // checks if user with user id belongs to team with team id
        Task<bool> ExistsInTeamAsync(int teamId, int userId);
    }
}
