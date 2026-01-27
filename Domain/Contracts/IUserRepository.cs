using Domain.Entities;
using Domain.Enums;

namespace Domain.Contracts
{
    public interface IUserRepository : IGenericRepository<User, int>
    {
        // get users by email
        Task<User?> GetByEmailAsync(string email, bool tracked = false);
        // get users by id with teams info
        Task<User?> GetUserWithTeamsEntityAsync(int userId);
        // gets all soft deleted entities by org id [paginated]
        Task<List<User>> GetAllSoftDeletedAsyncByOrgId(int orgId, int pageNumber = 1, int pageSize = 10, bool tracked = false);
    }
}