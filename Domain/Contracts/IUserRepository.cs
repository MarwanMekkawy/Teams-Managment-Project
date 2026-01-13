using Domain.Entities;
using Domain.Enums;


namespace Domain.Contracts
{
    public interface IUserRepository : IGenericRepository<User, int>
    {
        // get users by organization with optional role filter
        Task<IEnumerable<User>> GetByOrganizationAsync(int organizationId,UserRole? role = null, bool tracked = false);

        Task<User?> GetByEmailAsync(string email, bool tracked = false);
    }
}