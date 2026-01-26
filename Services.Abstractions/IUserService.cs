using Domain.Enums;
using Shared.Claims;
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
        Task<UserDto> GetByIdAsync(int id, UserClaims userCredentials);
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<UserDto> UpdateAsync(int id, UpdateUserDto dto);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id, UserClaims userCredentials);
        Task RestoreAsync(int id, UserClaims userCredentials);
        Task<List<UserDto>> GetAllDeletedUsersAsync(UserClaims userCredentials);


        Task<UserDto> GetByEmailAsync(string email, UserClaims userCredentials);
        Task<List<UserDto>> GetUsersByOrganizationAsync(int organizationId, UserRole? role = null);
        Task<List<UserDto>> GetUsersByTeamAsync(int teamId);
    }
}