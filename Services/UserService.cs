using AutoMapper;
using Domain.Contracts;
using Domain.Contracts.Security;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Services.Abstractions;
using Shared.Claims;
using Shared.UserDTOs;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IPasswordHasher Hasher;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher hasher)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.Hasher = hasher;
        }
        // Crud methods //
        public async Task<UserDto> GetByIdAsync(int id, UserClaims userCredentials)
        {
            var user = await unitOfWork.users.GetUserWithTeamsEntityAsync(id);
            if (user == null) throw new NotFoundException($"User with ID {id} not found");

            if ((userCredentials.Role == UserRole.Manager || userCredentials.Role == UserRole.TeamLeader) &&user.OrganizationId != userCredentials.OrgId) 
                throw new ForbiddenException("can only access users in your organization");

            return mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {          
            var newUser = mapper.Map<User>(dto);
            newUser.PasswordHash = Hasher.Hash(dto.Password);
            unitOfWork.users.Add(newUser);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<UserDto>(newUser);
        }

        public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto)
        {
            var existingUser = await unitOfWork.users.GetAsync(id);
            if (existingUser == null) throw new NotFoundException($"User with ID {id} not found");
            existingUser.Name = dto.Name ?? existingUser.Name;
            existingUser.Email = dto.Email ?? existingUser.Email;
            existingUser.Role = dto.Role ?? existingUser.Role;
            existingUser.OrganizationId = dto.OrganizationId ?? existingUser.OrganizationId;
            unitOfWork.users.Update(existingUser);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<UserDto>(existingUser);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await unitOfWork.users.GetAsync(id);
            if (user == null) throw new NotFoundException($"User with ID {id} not found");
            unitOfWork.users.Delete(user);
            await unitOfWork.SaveChangesAsync();
        }

        // Soft Delete methods //
        public async Task SoftDeleteAsync(int id, UserClaims userCredentials)
        {
            var user = await unitOfWork.users.GetAsync(id);
            if (user == null) throw new NotFoundException($"User with ID {id} not found");

            if (userCredentials.Role == UserRole.Manager && user.OrganizationId != userCredentials.OrgId)
                throw new ForbiddenException("Manger can only soft-delete users in his organization");

            user.IsDeleted = true;
            unitOfWork.users.Update(user);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task RestoreAsync(int id, UserClaims userCredentials)
        {
            var user = await unitOfWork.users.GetIncludingDeletedAsync(id);
            if (user == null) throw new NotFoundException($"user with ID {id} not found");
            if (!user.IsDeleted) throw new BadRequestException("the entity is Not a deleted Entity");

            if (userCredentials.Role == UserRole.Manager && user.OrganizationId != userCredentials.OrgId)
                throw new ForbiddenException("Manger can only restore softe-deleted users in his organization");

            user.IsDeleted = false;
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<UserDto>> GetAllDeletedUsersAsync(int pageNumber , int pageSize, UserClaims userCredentials)
        {
            if(userCredentials.Role == UserRole.Manager)
            {
                var orgDeletedUsers = await unitOfWork.users.GetAllSoftDeletedAsyncByOrgId(userCredentials.OrgId, pageNumber, pageSize);
                return mapper.Map<List<UserDto>>(orgDeletedUsers);
            }
            var deletedUsers = await unitOfWork.users.GetAllSoftDeletedAsync(pageNumber, pageSize);
            return mapper.Map<List<UserDto>>(deletedUsers);
        }

        // get methods related to another entity //
        public async Task<UserDto> GetByEmailAsync(string email, UserClaims userCredentials)
        {
            var user = await unitOfWork.users.GetByEmailAsync(email);
            if (user == null) throw new NotFoundException($"User with Email {email} not found");

            if ((userCredentials.Role == UserRole.Manager || userCredentials.Role == UserRole.TeamLeader) && user.OrganizationId != userCredentials.OrgId)
                throw new ForbiddenException("can only access users in your organization");

            return mapper.Map<UserDto>(user);

        }

        public async Task<List<UserDto>> GetUsersByOrganizationAsync(int organizationId, UserRole? role = null)
        {
            var users = await unitOfWork.users.GetByOrganizationAndRoleAsync(organizationId, role);
            if (users == null) return new List<UserDto>();                     // empty list
            return mapper.Map<List<UserDto>>(users);
        }

        public async Task<List<UserDto>> GetUsersByTeamAsync(int teamId)
        {
            var users = await unitOfWork.users.GetByTeamAsync(teamId);
            if (users == null) return new List<UserDto>();                     // empty list
            return mapper.Map<List<UserDto>>(users);
        }
    }
}
