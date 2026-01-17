using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Services.Abstractions;
using Shared.TeamDTOs;
using System;
using System.Collections.Generic;
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
            if (team == null) throw new KeyNotFoundException($"Team with ID {id} not found");
            return mapper.Map<TeamDto>(team);
        }

        public async Task<TeamDto> CreateAsync(CreateTeamDto dto)
        {
            var newTeam = mapper.Map<Team>(dto);
            unitOfWork.teams.Add(newTeam);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<TeamDto>(newTeam);
        }

        public async Task<TeamDto> UpdateAsync(int id, UpdateTeamDto dto)
        {
            var existingTeam = await unitOfWork.teams.GetAsync(id);
            if (existingTeam == null) throw new KeyNotFoundException($"Team with ID {id} not found");
            existingTeam.Name = dto.Name ?? existingTeam.Name;
            unitOfWork.teams.Update(existingTeam);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<TeamDto>(existingTeam);
        }

        public async Task DeleteAsync(int id)
        {
            var team = await unitOfWork.teams.GetAsync(id);
            if (team == null) throw new KeyNotFoundException($"Team with ID {id} not found");
            unitOfWork.teams.Delete(team);
            await unitOfWork.SaveChangesAsync();
        }

        // Soft Delete methods //
        public async Task SoftDeleteAsync(int id)
        {
            var team = await unitOfWork.teams.GetAsync(id);
            if (team == null) throw new KeyNotFoundException($"Team with ID {id} not found");
            team.IsDeleted = true;
            unitOfWork.teams.Update(team);
            await unitOfWork.SaveChangesAsync();
        }
        public async Task RestoreAsync(int id)
        {
            var team = await unitOfWork.teams.GetIncludingDeletedAsync(id);
            if (team == null) throw new KeyNotFoundException($"team with ID {id} not found");
            if (!team.IsDeleted) throw new InvalidOperationException("Not deleted Entity");

            team.IsDeleted = false;
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<TeamDto>> GetAllDeletedTeamsAsync()
        {
            var deletedTeams = await unitOfWork.teams.GetAllSoftDeletedAsync();
            return mapper.Map<List<TeamDto>>(deletedTeams);
        }

        // get methods related to another entity //
        public async Task<List<TeamDto>> GetTeamsByOrganizationAsync(int organizationId)
        {
            var teams= await unitOfWork.teams.GetByOrganizationAsync(organizationId);
            if (teams == null) return new List<TeamDto>();                     // empty list
            return mapper.Map<List<TeamDto>>(teams);
        }

        public async Task<List<TeamDto>> GetTeamsByUserAsync(int userId)
        {
            var teams = await unitOfWork.teams.GetByUserAsync(userId);
            if (teams == null) return new List<TeamDto>();                     // empty list
            return mapper.Map<List<TeamDto>>(teams);
        }
    }
}
