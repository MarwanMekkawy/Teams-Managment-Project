using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Services.Abstractions;
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
        public async Task<ProjectDto> GetByIdAsync(int id)
        {
            var project = await unitOfWork.projects.GetAsync(id);
            if (project == null) throw new NotFoundException($"Project with ID {id} not found");
            return mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
        {
            var project = mapper.Map<Project>(dto);
            unitOfWork.projects.Add(project);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> UpdateAsync(int id, UpdateProjectDto dto)
        {
            var project = await unitOfWork.projects.GetAsync(id);
            if (project == null) throw new NotFoundException($"Project with ID {id} not found");
            project.Status = dto.Status ?? project.Status;
            project.Name = dto.Name ?? project.Name;
            unitOfWork.projects.Update(project);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<ProjectDto>(project);
        }

        public async Task ChangeStatusAsync(int id, ProjectStatus newStatus)
        {
            var project = await unitOfWork.projects.GetAsync(id);
            if (project == null) throw new NotFoundException($"Project with ID {id} not found");
            project.Status = newStatus;
            unitOfWork.projects.Update(project);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var project = await unitOfWork.projects.GetAsync(id);
            if (project == null) throw new NotFoundException($"Project with ID {id} not found");
            unitOfWork.projects.Delete(project);
            await unitOfWork.SaveChangesAsync();
        }

        // Soft Delete methods //
        public async Task SoftDeleteAsync(int id)
        {
            var project = await unitOfWork.projects.GetAsync(id);
            if (project == null) throw new NotFoundException($"Project with ID {id} not found");
            project.IsDeleted = true;
            unitOfWork.projects.Update(project);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task RestoreAsync(int id)
        {
            var project = await unitOfWork.projects.GetIncludingDeletedAsync(id);
            if (project == null) throw new NotFoundException($"project with ID {id} not found");
            if (!project.IsDeleted) throw new BadRequestException("the Entity is Not a deleted Entity");

            project.IsDeleted = false;
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<ProjectDto>> GetAllDeletedProjectsAsync()
        {
            var deletedProjects = await unitOfWork.projects.GetAllSoftDeletedAsync();
            return mapper.Map<List<ProjectDto>>(deletedProjects);
        }

        // get methods related to another entity //
        public async Task<List<ProjectDto>> GetByTeamAsync(int teamId)
        {
            var projects =await unitOfWork.projects.GetByTeamAsync(teamId);
            return mapper.Map<List<ProjectDto>>(projects);
        }

        public async Task<List<ProjectDto>> GetByOrganizationAsync(int organizationId)
        {
            var projects = await unitOfWork.projects.GetByOrganizationAndStatusAsync(organizationId);
            return mapper.Map<List<ProjectDto>>(projects);
        }       
    }
}
