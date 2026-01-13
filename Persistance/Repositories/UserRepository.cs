using Domain.Contracts;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace Persistance.Repositories
{
    public class UserRepository : GenericRepository<User , int> , IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<User>> GetByOrganizationAsync(int organizationId, UserRole? role = null, bool tracked = false)
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
    }
}
