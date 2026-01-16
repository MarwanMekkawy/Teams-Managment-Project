using Domain.Enums;
using Shared.OrganizationDTOs;
using Shared.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<UserDto> UpdateAsync(int id, UpdateUserDto dto);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task RestoreAsync(int id);
        Task<List<UserDto>> GetAllDeletedUsersAsync();


        Task<UserDto> GetByEmailAsync(string email);
        Task<List<UserDto>> GetUsersByOrganizationAsync(int organizationId, UserRole? role = null);
        Task<List<UserDto>> GetUsersByTeamAsync(int teamId);
    }
}