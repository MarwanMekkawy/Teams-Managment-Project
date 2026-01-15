using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class TeamMemberRepository : ITeamMemberRepository
    {
        private readonly AppDbContext _context;
        public TeamMemberRepository(AppDbContext context) 
        {
            _context = context;
        }
        public async Task<TeamMember> GetByTeamAndUserAsync(int teamId, int userId)
        {
            var teamMember = await _context.TeamMembers.FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId);
            return teamMember;
        }

        public void AddMember(TeamMember entity)
        {
            _context.TeamMembers.AddAsync(entity);
        }

        public void RemoveMember(TeamMember entity)
        {
            _context.TeamMembers.Remove(entity);
        }

        public async Task<IEnumerable<int>> GetUserIdsByTeamAsync(int teamId)
        {
            return await _context.TeamMembers.Where(tm => tm.TeamId == teamId).Select(tm => tm.UserId).ToListAsync();
        }

        public async Task<IEnumerable<int>> GetTeamIdsByUserAsync(int userId)
        {
            return await _context.TeamMembers.Where(tm=>tm.UserId == userId).Select(tm => tm.TeamId).ToListAsync();
        }

        public async Task<bool> ExistsAsync(int teamId, int userId)
        {
            return await _context.TeamMembers.AnyAsync(tm=>tm.TeamId == teamId && tm.UserId == userId );
        }
    }
}
