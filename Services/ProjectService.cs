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
        public Task<ProjectDto> CreateAsync(CreateProjectDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProjectDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProjectDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProjectDto>> GetProjectsByTeamAsync(int teamId)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectDto> UpdateAsync(int id, UpdateProjectDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
