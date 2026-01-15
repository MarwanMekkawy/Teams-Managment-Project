using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Services.Abstractions;
using Shared.TeamMemberDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public TeamMemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task AddMemberAsync(CreateTeamMemberDto dto)
        {
            var useExistsInTeam = await unitOfWork.teamMembers.ExistsAsync(dto.TeamId, dto.UserId);
            if (useExistsInTeam) throw new Exception($"The user with ID {dto.UserId} already a member of the team with ID {dto.TeamId}");
            var teamMember = mapper.Map<TeamMember>(dto);
            unitOfWork.teamMembers.AddMember(teamMember);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(int teamId, int userId)
        {
            var userExists = await unitOfWork.users.ExistsAsync(userId);
            if (!userExists) throw new KeyNotFoundException($"User with ID {userId} not found");

            var teamExists = await unitOfWork.teams.ExistsAsync(teamId);
            if (!teamExists) throw new KeyNotFoundException($"Team with ID {teamId} not found");

            var existingTeamMember = await unitOfWork.teamMembers.GetByTeamAndUserAsync(teamId, userId);
            if (existingTeamMember == null) throw new KeyNotFoundException($"Team member with ID {userId} not found in Team with ID {teamId}");

            unitOfWork.teamMembers.RemoveMember(existingTeamMember);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsMemberAsync(int teamId, int userId)
        {
            var userExists = await unitOfWork.users.ExistsAsync(userId);
            if (!userExists) throw new KeyNotFoundException($"User with ID {userId} not found");

            var teamExists = await unitOfWork.teams.ExistsAsync(teamId);
            if (!teamExists) throw new KeyNotFoundException($"Team with ID {teamId} not found");

            var existingTeamMember = await unitOfWork.teamMembers.GetByTeamAndUserAsync(teamId, userId);
            if (existingTeamMember == null) return false;
            return true;
        }   
    }
}
