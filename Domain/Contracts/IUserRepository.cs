using Domain.Entities;
using Domain.Enums;

namespace Domain.Contracts
{
    public interface IUserRepository : IGenericRepository<User, int>
    {
        // get users by organization with optional role filter
        Task<IEnumerable<User>> GetByOrganizationAndRoleAsync(int organizationId,UserRole? role = null, bool tracked = false);
        // get users by email
        Task<User?> GetByEmailAsync(string email, bool tracked = false);
        // get users by team id
        Task<IEnumerable<User>> GetByTeamAsync(int teamId, bool tracked = false);
        // get users by id with teams info
        Task<User?> GetUserWithTeamsEntityAsync(int userId);
    }
}