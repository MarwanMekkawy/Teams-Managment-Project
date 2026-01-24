using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.ProjectDTOs;

namespace TeamsManagmentProject.API.Controllers
{
    /// <summary>
    /// Exposes endpoints for managing projects and their lifecycle.
    /// </summary>
    [ApiController]
    [Route("api/v1/projects")]
    public class ProjectsController(IProjectService _service) : ControllerBase
    {
        /// <summary>
        /// Retrieves a specific project by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the project.</param>
        /// <returns>The requested project.</returns>
        /// <response code="200">Project retrieved successfully.</response>
        /// <response code="404">Project not found.</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager,Member")]
        public async Task<ActionResult<ProjectDto>> GetById(int id)
            => Ok(await _service.GetByIdAsync(id));

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="dto">The data required to create the project.</param>
        /// <returns>The newly created project.</returns>
        /// <response code="201">Project created successfully.</response>
        /// <response code="400">Invalid input data.</response>
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto dto)
        {
            var createdProject = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdProject.Id }, createdProject);
        }

        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="id">The unique identifier of the project.</param>
        /// <param name="dto">The updated project data.</param>
        /// <returns>The updated project.</returns>
        /// <response code="200">Project updated successfully.</response>
        /// <response code="404">Project not found.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<ProjectDto>> Update(int id, UpdateProjectDto dto)
            => Ok(await _service.UpdateAsync(id, dto));

        /// <summary>
        /// Changes the status of a project.
        /// </summary>
        /// <param name="id">The unique identifier of the project.</param>
        /// <param name="status">The new status to apply.</param>
        /// <response code="204">Project status updated successfully.</response>
        /// <response code="404">Project not found.</response>
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ChangeStatus(int id, ProjectStatus status)
        {
            await _service.ChangeStatusAsync(id, status);
            return NoContent();
        }

        /// <summary>
        /// Permanently deletes a project.
        /// </summary>
        /// <param name="id">The unique identifier of the project.</param>
        /// <response code="204">Project deleted successfully.</response>
        /// <response code="404">Project not found.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Soft-deletes a project without permanently removing it.
        /// </summary>
        /// <param name="id">The unique identifier of the project.</param>
        /// <response code="204">Project soft-deleted successfully.</response>
        /// <response code="404">Project not found.</response>
        [HttpPatch("{id}/soft-delete")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _service.SoftDeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Restores a previously soft-deleted project.
        /// </summary>
        /// <param name="id">The unique identifier of the project.</param>
        /// <response code="204">Project restored successfully.</response>
        /// <response code="404">Project not found.</response>
        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Restore(int id)
        {
            await _service.RestoreAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Retrieves all soft-deleted projects.
        /// </summary>
        /// <returns>A list of soft-deleted projects.</returns>
        /// <response code="200">Soft-deleted projects retrieved successfully.</response>
        [HttpGet("soft-deleted")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ProjectDto>>> GetDeleted()
            => Ok(await _service.GetAllDeletedProjectsAsync());

        /// <summary>
        /// Retrieves all projects associated with a specific team.
        /// </summary>
        /// <param name="teamId">The unique identifier of the team.</param>
        /// <returns>A list of projects for the team.</returns>
        /// <response code="200">Projects retrieved successfully.</response>
        [HttpGet("by-team/{teamId}")]
        [Authorize(Roles = "Admin,Manager,Member")]
        public async Task<ActionResult<List<ProjectDto>>> GetByTeam(int teamId)
            => Ok(await _service.GetByTeamAsync(teamId));

        /// <summary>
        /// Retrieves all projects associated with a specific organization.
        /// </summary>
        /// <param name="organizationId">The unique identifier of the organization.</param>
        /// <returns>A list of projects for the organization.</returns>
        /// <response code="200">Projects retrieved successfully.</response>
        [HttpGet("by-organization/{organizationId}")]
        [Authorize(Roles = "Admin,Manager,Member")]
        public async Task<ActionResult<List<ProjectDto>>> GetByOrganization(int organizationId)
            => Ok(await _service.GetByOrganizationAsync(organizationId));
    }

}
