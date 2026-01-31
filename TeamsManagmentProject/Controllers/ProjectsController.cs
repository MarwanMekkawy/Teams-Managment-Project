using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.ProjectDTOs;
using TeamsManagmentProject.API.ApiClaimsFactory;

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
        /// <param name="id">The project identifier.</param>
        /// <response code="200">Project retrieved successfully.</response>
        /// <response code="404">Project not found.</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager,TeamLeader,Member")]
        public async Task<ActionResult<ProjectDto>> GetById(int id)
        {
            var ctx = UserClaimsFactory.From(User);
            return Ok(await _service.GetByIdAsync(id, ctx));
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="dto">Project creation data.</param>
        /// <response code="201">Project created successfully.</response>
        /// <response code="400">Invalid input data.</response>
        [HttpPost]
        [Authorize(Roles = "Admin,Manager,TeamLeader")]
        public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto dto)
        {
            var ctx = UserClaimsFactory.From(User);
            var createdProject = await _service.CreateAsync(dto, ctx);
            return CreatedAtAction(nameof(GetById), new { id = createdProject.Id }, createdProject);
        }

        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="id">The project identifier.</param>
        /// <param name="dto">Updated project data.</param>
        /// <response code="200">Project updated successfully.</response>
        /// <response code="404">Project not found.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager,TeamLeader")]
        public async Task<ActionResult<ProjectDto>> Update(int id, UpdateProjectDto dto)
        {
            var ctx = UserClaimsFactory.From(User);
            return Ok(await _service.UpdateAsync(id, dto, ctx));
        }

        /// <summary>
        /// Changes the status of a project.
        /// </summary>
        /// <param name="id">The project identifier.</param>
        /// <param name="dto">The new status to apply.</param>
        /// <response code="204">Project status updated successfully.</response>
        /// <response code="404">Project not found.</response>
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,Manager,TeamLeader")]
        public async Task<IActionResult> ChangeStatus(int id, ChangeProjectStatusDto dto)
        {
            var ctx = UserClaimsFactory.From(User);
            await _service.ChangeStatusAsync(id, dto.Status, ctx);
            return NoContent();
        }

        /// <summary>
        /// Permanently deletes a project.
        /// </summary>
        /// <param name="id">The project identifier.</param>
        /// <response code="204">Project deleted successfully.</response>
        /// <response code="404">Project not found.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var ctx = UserClaimsFactory.From(User);
            await _service.DeleteAsync(id, ctx);
            return NoContent();
        }

        /// <summary>
        /// Soft-deletes a project without permanently removing it.
        /// </summary>
        /// <param name="id">The project identifier.</param>
        /// <response code="204">Project soft-deleted successfully.</response>
        /// <response code="404">Project not found.</response>
        [HttpPatch("{id}/soft-delete")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var ctx = UserClaimsFactory.From(User);
            await _service.SoftDeleteAsync(id, ctx);
            return NoContent();
        }

        /// <summary>
        /// Restores a previously soft-deleted project.
        /// </summary>
        /// <param name="id">The project identifier.</param>
        /// <response code="204">Project restored successfully.</response>
        /// <response code="404">Project not found.</response>
        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Restore(int id)
        {
            var ctx = UserClaimsFactory.From(User);
            await _service.RestoreAsync(id, ctx);
            return NoContent();
        }

        /// <summary>
        /// Retrieves all soft-deleted projects.[paginated]
        /// </summary>
        /// <response code="200">Soft-deleted projects retrieved successfully.</response>
        [HttpGet("soft-deleted")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ProjectDto>>> GetDeleted(int pageNumber = 1, int pageSize = 10)
        {
            return Ok(await _service.GetAllDeletedProjectsAsync(pageNumber, pageSize));
        }

        /// <summary>
        /// Retrieves all projects associated with a specific team.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <response code="200">Projects retrieved successfully.</response>
        [HttpGet("by-team/{teamId}")]
        [Authorize(Roles = "Admin,Manager,TeamLeader")]
        public async Task<ActionResult<List<ProjectDto>>> GetByTeam(int teamId)
        {
            var ctx = UserClaimsFactory.From(User);
            return Ok(await _service.GetByTeamAsync(teamId, ctx));
        }

        /// <summary>
        /// Retrieves all projects associated with a specific organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <response code="200">Projects retrieved successfully.</response>
        [HttpGet("by-organization/{organizationId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<List<ProjectDto>>> GetByOrganization(int organizationId)
        {
            var ctx = UserClaimsFactory.From(User);
            return Ok(await _service.GetByOrganizationAsync(organizationId, ctx));
        }
    }
}
