using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Services.Abstractions;
using Shared.Claims;
using Shared.TeamMemberDTOs;


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
        public async Task AddMemberAsync(CreateTeamMemberDto dto, UserClaims userCredentials)
        {
            var team = await unitOfWork.teams.GetAsync(dto.TeamId);
            if (team == null)
                throw new NotFoundException($"Team with ID {dto.TeamId} not found");

            switch (userCredentials.Role)
            {
                case UserRole.Admin:
                    break;

                case UserRole.Manager:
                    if (team.OrganizationId != userCredentials.OrgId) throw new
                            ForbiddenException("Managers can only add members to teams in their organization");
                    break;

                case UserRole.TeamLeader:
                    var isLeaderInTeam = await unitOfWork.teamMembers.ExistsInTeamAsync(dto.TeamId, userCredentials.UserId);
                    if (!isLeaderInTeam)throw new ForbiddenException("Team leaders can only add members to teams they belong to");
                    break;

                default:
                    throw new ForbiddenException("You are not allowed to access team members");
            }

            var userExistsInTeam = await unitOfWork.teamMembers.ExistsInTeamAsync(dto.TeamId, dto.UserId);

            if (userExistsInTeam)  throw new ConflictException
                    ($"The user with ID {dto.UserId} is already a member of the team with ID {dto.TeamId}");

            var teamMember = mapper.Map<TeamMember>(dto);
            unitOfWork.teamMembers.AddMember(teamMember);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(int teamId, int userId, UserClaims userCredentials)
        {
            var team = await unitOfWork.teams.GetAsync(teamId);
            if (team == null) throw new NotFoundException($"Team with ID {teamId} not found");

            var teamMember = await unitOfWork.teamMembers.GetByTeamAndUserAsync(teamId, userId);
            if (teamMember == null) throw new NotFoundException($"User with ID {userId} is not a member of team with ID {teamId}");

            switch (userCredentials.Role)
            {
                case UserRole.Admin:
                    break;

                case UserRole.Manager:
                    if (team.OrganizationId != userCredentials.OrgId) throw new 
                            ForbiddenException( "Managers can only remove members from teams in their organization");
                    break;

                case UserRole.TeamLeader:
                    var isLeaderInTeam = await unitOfWork.teamMembers.ExistsInTeamAsync(teamId, userCredentials.UserId);
                    if (!isLeaderInTeam) throw new ForbiddenException("Team leaders can only manage teams they belong to");
                    break;

                default:
                    throw new ForbiddenException("You are not allowed to access team members");
            }

            unitOfWork.teamMembers.RemoveMember(teamMember);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsMemberAsync(int teamId, int userId, UserClaims userCredentials)
        {
            var team = await unitOfWork.teams.GetAsync(teamId);
            if (team == null) throw new NotFoundException($"Team with ID {teamId} not found");

            switch (userCredentials.Role)
            {
                case UserRole.Admin:
                    break;

                case UserRole.Manager:
                    if (team.OrganizationId != userCredentials.OrgId) throw new 
                            ForbiddenException("Managers can only access teams in their organization");
                    break;

                case UserRole.TeamLeader:
                    var isLeaderInTeam = await unitOfWork.teamMembers.ExistsInTeamAsync(teamId, userCredentials.UserId); 

                    if (!isLeaderInTeam)
                        throw new ForbiddenException(
                            "Team leaders can only access teams they belong to");
                    break;

                case UserRole.Member:
                    if (userCredentials.UserId != userId) throw new ForbiddenException("Members can only check their own team membership");
                    break;

                default:
                    throw new ForbiddenException("You are not allowed to access team members");
            }

            return await unitOfWork.teamMembers.ExistsInTeamAsync(teamId, userId);
        }   
    }
}
