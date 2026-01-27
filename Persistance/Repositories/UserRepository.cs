using Domain.Contracts;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Shared.TeamMemberDTOs;
using Shared.UserDTOs;


namespace Persistance.Repositories
{
    public class UserRepository : GenericRepository<User , int> , IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<User>> GetByOrganizationAndRoleAsync(int organizationId, UserRole? role = null, bool tracked = false)
        {
            var query = _context.Users.Where(u => u.OrganizationId == organizationId).AsQueryable();
            if (!tracked) query = query.AsNoTracking();
            if (role != null) query =query.Where(u=>u.Role == role);
            return await query.ToListAsync();   
        }

        public async Task<User?> GetByEmailAsync(string email, bool tracked = false)
        {
            var query = _context.Users.Where(u => u.Email == email).AsQueryable();
            if (!tracked) query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<User>> GetByTeamAsync(int teamId, bool tracked = false)
        {
            var query = _context.Users.Where(u=>u.TeamMemberships.Any(t=>t.TeamId == teamId)).AsQueryable();
            if (!tracked) query = query.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<User?> GetUserWithTeamsEntityAsync(int userId)
        {
            return await _context.Users
            .Include(u => u.TeamMemberships).ThenInclude(tm => tm.Team).Where(u => u.Id == userId).AsNoTracking().FirstOrDefaultAsync();
        }
      
        public async Task<List<User>> GetAllSoftDeletedAsyncByOrgId(int orgId, int pageNumber = 1, int pageSize = 10, bool tracked = false)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.Users.IgnoreQueryFilters().Where(e => e.IsDeleted && e.OrganizationId == orgId);
            if (!tracked) query = query.AsNoTracking();
            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
