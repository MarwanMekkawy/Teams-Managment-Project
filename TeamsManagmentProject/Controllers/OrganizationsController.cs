using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Claims;
using Shared.OrganizationDTOs;
using TeamsManagmentProject.API.ApiClaimsFactory;

namespace TeamsManagmentProject.API.Controllers
{

    /// <summary>
    /// Exposes endpoints for managing organizations and organization-level operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/organizations")]
    public class OrganizationsController(IOrganizationService _service) : ControllerBase
    {
        /// <summary>
        /// Retrieves aggregated statistics for the organization associated with the authenticated manager.
        /// </summary>
        /// <response code="200">Statistics retrieved successfully.</response>
        /// <response code="404">Organization not found.</response>
        [HttpGet("stats")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetStats()
        {
            // extracting claims from requesting user
            var ctx = UserClaimsFactory.From(User);

            var orgStats = await _service.GetStatsAsync(ctx.OrgId);
            var statsObject = new
            {
                totalUsers = orgStats.totalUsers,
                totalTeams = orgStats.totalTeams,
                activeProjects = orgStats.activeProjects,
                archivedProjects = orgStats.archivedProjects,
                totalTasks = orgStats.totalTasks,
                completedTasks = orgStats.completedTasks,
                overdueTasks = orgStats.overdueTasks
            };
            return Ok(statsObject);
        }

        /// <summary>
        /// [Admin] Retrieves aggregated statistics for a specific organization.
        /// </summary>
        /// <param name="id">The organization identifier.</param>
        /// <response code="200">Statistics retrieved successfully.</response>
        /// <response code="404">Organization not found.</response>
        [HttpGet("{id}/stats")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStats(int id)
        {
            var orgStats = await _service.GetStatsAsync(id);
            var statsObject = new
            {
                totalUsers = orgStats.totalUsers,
                totalTeams = orgStats.totalTeams,
                activeProjects = orgStats.activeProjects,
                archivedProjects = orgStats.archivedProjects,
                totalTasks = orgStats.totalTasks,
                completedTasks = orgStats.completedTasks,
                overdueTasks = orgStats.overdueTasks
            };
            return Ok(statsObject);
        }

        /// <summary>
        /// [Admin] Retrieves organizations with pagination.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <response code="200">Organizations retrieved successfully.</response>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<string>>> GetAll(int pageNumber = 1, int pageSize = 5)
            => Ok(await _service.GetAllAsync(pageNumber, pageSize));

        /// <summary>
        /// [Admin] Retrieves a specific organization by its identifier.
        /// </summary>
        /// <param name="id">The organization identifier.</param>
        /// <response code="200">Organization retrieved successfully.</response>
        /// <response code="404">Organization not found.</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> GetById(int id)
            => Ok(await _service.GetByIdAsync(id));

        /// <summary>
        /// [Admin] Creates a new organization.
        /// </summary>
        /// <param name="dto">Organization creation data.</param>
        /// <response code="201">Organization created successfully.</response>
        /// <response code="400">Invalid input data.</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrganizationDto>> Create(CreateOrganizationDto dto)
        {
            var createdOrg = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdOrg.Id }, createdOrg);
        }

        /// <summary>
        /// [Admin] Updates an existing organization.
        /// </summary>
        /// <param name="id">The organization identifier.</param>
        /// <param name="dto">Updated organization data.</param>
        /// <response code="200">Organization updated successfully.</response>
        /// <response code="404">Organization not found.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrganizationDto>> Update(int id, UpdateOrganizationDto dto)
            => Ok(await _service.UpdateAsync(id, dto));

        /// <summary>
        /// [Admin] Permanently deletes an organization.
        /// </summary>
        /// <param name="id">The organization identifier.</param>
        /// <response code="204">Organization deleted successfully.</response>
        /// <response code="404">Organization not found.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// [Admin] Soft-deletes an organization.
        /// </summary>
        /// <param name="id">The organization identifier.</param>
        /// <response code="204">Organization soft-deleted successfully.</response>
        /// <response code="404">Organization not found.</response>
        [HttpPatch("{id}/soft-delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _service.SoftDeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// [Admin] Restores a previously soft-deleted organization.
        /// </summary>
        /// <param name="id">The organization identifier.</param>
        /// <response code="204">Organization restored successfully.</response>
        /// <response code="404">Organization not found.</response>
        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Restore(int id)
        {
            await _service.RestoreAsync(id);
            return NoContent();
        }

        /// <summary>
        /// [Admin] Retrieves all soft-deleted organizations.
        /// </summary>
        /// <response code="200">Soft-deleted organizations retrieved successfully.</response>
        [HttpGet("soft-deleted")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<OrganizationDto>>> GetDeleted()
            => Ok(await _service.GetAllDeletedOrganizationsAsync());
    }
}
