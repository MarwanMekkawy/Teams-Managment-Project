using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Services.Abstractions;
using Shared.Claims;
using Shared.TeamDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TeamService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        // Crud methods //
        public async Task<TeamDto> GetByIdAsync(int id)
        {
            var team = await unitOfWork.teams.GetAsync(id);
            if (team == null) throw new NotFoundException($"Team with ID {id} not found or soft-deleted");
            return mapper.Map<TeamDto>(team);
        }

        public async Task<TeamDto> CreateAsync(CreateTeamDto dto, UserClaims userCredentials)
        {
            if (userCredentials.Role == UserRole.Manager) dto.OrganizationId = userCredentials.OrgId;

            var newTeam = mapper.Map<Team>(dto);
            unitOfWork.teams.Add(newTeam);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<TeamDto>(newTeam);
        }

        public async Task<TeamDto> UpdateAsync(int id, UpdateTeamDto dto, UserClaims userCredentials)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new BadRequestException("Team name cannot be empty");          

            var existingTeam = await unitOfWork.teams.GetAsync(id);
            if (existingTeam == null) throw new NotFoundException($"Team with ID {id} not found");

            if (userCredentials.Role == UserRole.Manager && existingTeam.OrganizationId != userCredentials.OrgId)
                throw new ForbiddenException("Managers can only update teams in their own organization");
           
            existingTeam.Name = dto.Name ?? existingTeam.Name;
            unitOfWork.teams.Update(existingTeam);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<TeamDto>(existingTeam);
        }

        public async Task DeleteAsync(int id)
        {
            var team = await unitOfWork.teams.GetAsync(id);
            if (team == null) throw new NotFoundException($"Team with ID {id} not found");
            unitOfWork.teams.Delete(team);
            await unitOfWork.SaveChangesAsync();
        }

        // Soft Delete methods //
        public async Task SoftDeleteAsync(int id, UserClaims userCredentials)
        {
            var team = await unitOfWork.teams.GetAsync(id);
            if (team == null) throw new NotFoundException($"Team with ID {id} not found");
            if (team.IsDeleted) throw new BadRequestException("Team is already deleted.");

            if (userCredentials.Role == UserRole.Manager && team.OrganizationId != userCredentials.OrgId)
                throw new ForbiddenException("Managers can only Delete teams in their own organization");

            team.IsDeleted = true;
            unitOfWork.teams.Update(team);
            await unitOfWork.SaveChangesAsync();
        }
        public async Task RestoreAsync(int id, UserClaims userCredentials)
        {
            var team = await unitOfWork.teams.GetIncludingDeletedAsync(id);
            if (team == null) throw new NotFoundException($"team with ID {id} not found");
            if (!team.IsDeleted) throw new BadRequestException("the entity is Not a deleted Entity");

            if (userCredentials.Role == UserRole.Manager && team.OrganizationId != userCredentials.OrgId)
                throw new ForbiddenException("Managers can only Restore teams in their own organization");


            team.IsDeleted = false;
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<TeamDto>> GetAllDeletedTeamsAsync()
        {
            var deletedTeams = await unitOfWork.teams.GetAllSoftDeletedAsync();
            return mapper.Map<List<TeamDto>>(deletedTeams);
        }

        // get methods related to another entity //
        public async Task<List<TeamDto>> GetTeamsByOrganizationAsync(int organizationId, UserClaims userCredentials)
        {
            if (userCredentials.Role == UserRole.Manager && organizationId != userCredentials.OrgId)
                throw new ForbiddenException("Managers can only access their organization");

            var teams= await unitOfWork.teams.GetByOrganizationAsync(organizationId);
            return mapper.Map<List<TeamDto>>(teams);
        }

        public async Task<List<TeamDto>> GetTeamsByUserAsync(int userId, UserClaims userCredentials)
        {
            if ((userCredentials.Role == UserRole.Member || userCredentials.Role == UserRole.TeamLeader) && userId != userCredentials.UserId)
                throw new ForbiddenException("can only acces your teams");

            if (userCredentials.Role == UserRole.Manager)
            {
                var orgTeamsForId = await unitOfWork.teams.GetByUserAndOrganizationAsync(userId, userCredentials.OrgId);
                if(!orgTeamsForId.Any()) throw new ForbiddenException("The user does not belong to your organization or has no teams in it");
                return mapper.Map<List<TeamDto>>(orgTeamsForId);
            }
            
            var teams = await unitOfWork.teams.GetByUserAsync(userId);
            return mapper.Map<List<TeamDto>>(teams);
        }

        // get teams by role [manger/teamleader]
        public async Task<List<TeamDto>> GetTeamsByUserCredentialsAsync(UserClaims userCredentials)
        {
            if (userCredentials.Role == UserRole.Manager) return await GetTeamsByOrganizationAsync(userCredentials.OrgId, userCredentials);
            return await GetTeamsByUserAsync(userCredentials.UserId, userCredentials);
        }
    }
}
