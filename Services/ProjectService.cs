using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Services.Abstractions;
using Shared.Claims;
using Shared.ProjectDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ProjectService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        // Crud methods //
        public async Task<ProjectDto> GetByIdAsync(int id, UserClaims userCredentials)
        {

            var project = await unitOfWork.projects.GetAsync(id);
            if (project == null) throw new NotFoundException($"Project with ID {id} not found");

            switch (userCredentials.Role)
            {
                case UserRole.Admin:
                    break;

                case UserRole.Manager:
                    var teamInOrg = await unitOfWork.teams.IsInOrganization(project.TeamId,userCredentials.OrgId);
                    if (!teamInOrg) throw new ForbiddenException("Managers can only access projects in teams within their organization");
                    break;

                case UserRole.TeamLeader:
                case UserRole.Member:
                    var isInTeam = await unitOfWork.teamMembers.ExistsInTeamAsync(project.TeamId,userCredentials.UserId);
                    if (!isInTeam) throw new ForbiddenException("You can only access projects in teams you belong to");
                    break;

                default:
                    throw new ForbiddenException("You are not allowed to access this project");
            }

            return mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto, UserClaims userCredentials)
        {
            switch (userCredentials.Role)
            {
                case UserRole.Admin:
                    break;

                case UserRole.Manager:
                    var teamInOrg = await unitOfWork.teams.IsInOrganization(dto.TeamId,userCredentials.OrgId);
                    if (!teamInOrg) throw new ForbiddenException("Managers can only create projects in teams within their organization");
                    break;

                case UserRole.TeamLeader:
                    var isInTeam = await unitOfWork.teamMembers.ExistsInTeamAsync(dto.TeamId,userCredentials.UserId);
                    if (!isInTeam) throw new ForbiddenException("You can only create projects in teams you belong to");
                    break;

                default:
                    throw new ForbiddenException("You are not allowed to create this project");
            }

            var project = mapper.Map<Project>(dto);
            unitOfWork.projects.Add(project);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> UpdateAsync(int id, UpdateProjectDto dto, UserClaims userCredentials)
        {
            var project = await unitOfWork.projects.GetAsync(id);
            if (project == null) throw new NotFoundException($"Project with ID {id} not found");

            switch (userCredentials.Role)
            {
                case UserRole.Admin:
                    break;

                case UserRole.Manager:
                    var teamInOrg = await unitOfWork.teams.IsInOrganization(project.TeamId, userCredentials.OrgId);
                    if (!teamInOrg) throw new ForbiddenException("Managers can only update projects in teams within their organization");
                    break;

                case UserRole.TeamLeader:
                    var isInTeam = await unitOfWork.teamMembers.ExistsInTeamAsync(project.TeamId, userCredentials.UserId);
                    if (!isInTeam) throw new ForbiddenException("You can only update projects in teams you belong to");
                    break;

                default:
                    throw new ForbiddenException("You are not allowed to update this project");
            }

            project.Status = dto.Status ?? project.Status;
            project.Name = dto.Name ?? project.Name;
            unitOfWork.projects.Update(project);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<ProjectDto>(project);
        }

        public async Task ChangeStatusAsync(int id, ProjectStatus newStatus, UserClaims userCredentials)
        {
            var project = await unitOfWork.projects.GetAsync(id);
            if (project == null) throw new NotFoundException($"Project with ID {id} not found");

            switch (userCredentials.Role)
            {
                case UserRole.Admin:
                    break;

                case UserRole.Manager:
                    var teamInOrg = await unitOfWork.teams.IsInOrganization(project.TeamId, userCredentials.OrgId);
                    if (!teamInOrg) throw new ForbiddenException("Managers can only edit projects in teams within their organization");
                    break;

                case UserRole.TeamLeader:
                    var isInTeam = await unitOfWork.teamMembers.ExistsInTeamAsync(project.TeamId, userCredentials.UserId);
                    if (!isInTeam) throw new ForbiddenException("You can only edit projects in teams you belong to");
                    break;

                default:
                    throw new ForbiddenException("You are not allowed to edit this project");
            }

            project.Status = newStatus;
            unitOfWork.projects.Update(project);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, UserClaims userCredentials)
        {
            var project = await unitOfWork.projects.GetAsync(id);
            if (project == null) throw new NotFoundException($"Project with ID {id} not found");

            if (userCredentials.Role == UserRole.Manager)
            {
                var teamInOrg = await unitOfWork.teams.IsInOrganization(project.TeamId, userCredentials.OrgId);
                if (!teamInOrg) throw new ForbiddenException("Managers can only delet projects in teams within their organization");
            }

            unitOfWork.projects.Delete(project);
            await unitOfWork.SaveChangesAsync();
        }

        // Soft Delete methods //
        public async Task SoftDeleteAsync(int id, UserClaims userCredentials)
        {
            var project = await unitOfWork.projects.GetAsync(id);
            if (project == null) throw new NotFoundException($"Project with ID {id} not found");

            if (userCredentials.Role == UserRole.Manager)
            {
                var teamInOrg = await unitOfWork.teams.IsInOrganization(project.TeamId, userCredentials.OrgId);
                if (!teamInOrg) throw new ForbiddenException("Managers can only soft-delete projects in teams within their organization");
            }

            project.IsDeleted = true;
            unitOfWork.projects.Update(project);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task RestoreAsync(int id, UserClaims userCredentials)
        {
            var project = await unitOfWork.projects.GetIncludingDeletedAsync(id);
            if (project == null) throw new NotFoundException($"project with ID {id} not found");
            if (!project.IsDeleted) throw new BadRequestException("the Entity is Not a deleted Entity");

            if (userCredentials.Role == UserRole.Manager)
            {
                var teamInOrg = await unitOfWork.teams.IsInOrganization(project.TeamId, userCredentials.OrgId);
                if (!teamInOrg) throw new ForbiddenException("Managers can only restore projects in teams within their organization");
            }

            project.IsDeleted = false;
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<ProjectDto>> GetAllDeletedProjectsAsync()
        {
            var deletedProjects = await unitOfWork.projects.GetAllSoftDeletedAsync();
            return mapper.Map<List<ProjectDto>>(deletedProjects);
        }

        // get methods related to another entity //
        public async Task<List<ProjectDto>> GetByTeamAsync(int teamId, UserClaims userCredentials)
        {
            switch (userCredentials.Role)
            {
                case UserRole.Admin:                   
                    break;

                case UserRole.Manager:
                    var teamInOrg = await unitOfWork.teams.IsInOrganization(teamId, userCredentials.OrgId);
                    if (!teamInOrg) throw new ForbiddenException("Managers can only access projects in teams within their organization");
                    break;

                case UserRole.TeamLeader:                   
                    var isInTeam = await unitOfWork.teamMembers.ExistsInTeamAsync(teamId, userCredentials.UserId);
                    if (!isInTeam)throw new ForbiddenException("Team leaders can only access projects in teams they belong to");
                    break;

                default:
                    throw new ForbiddenException("You are not allowed to access these projects");
            }

            var projects =await unitOfWork.projects.GetByTeamAsync(teamId);
            return mapper.Map<List<ProjectDto>>(projects);
        }

        public async Task<List<ProjectDto>> GetByOrganizationAsync(int organizationId, UserClaims userCredentials)
        {
            if (userCredentials.Role == UserRole.Manager && userCredentials.OrgId != organizationId) 
                 throw new ForbiddenException("Managers can only access projects in teams within their organization");

            var projects = await unitOfWork.projects.GetByOrganizationAndStatusAsync(organizationId);
            return mapper.Map<List<ProjectDto>>(projects);
        }       
    }
}
