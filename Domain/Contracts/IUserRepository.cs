using Domain.Entities;
using Domain.Enums;


namespace Domain.Contracts
{
    public interface IUserRepository : IGenericRepository<User, int>
    {
        Task<IEnumerable<User>> GetByOrganizationAsync(int organizationId, bool tracked = false);
        Task<IEnumerable<User>> GetByOrganizationAndRoleAsync(int organizationId, UserRole role, bool tracked = false);
        Task<User?> GetByEmailAsync(string email, bool tracked = false);
    }
}