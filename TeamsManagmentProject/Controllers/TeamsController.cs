using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.TeamDTOs;
using System.Data;
using TeamsManagmentProject.API.ApiClaimsFactory;

namespace TeamsManagmentProject.API.Controllers
{
    /// <summary>
    /// Exposes endpoints for managing teams.
    /// </summary>
    [ApiController]
    [Route("api/v1/teams")]
    public class TeamsController(ITeamService _service) : ControllerBase
    {
        /// <summary>
        /// Retrieves teams by request user role.
        /// </summary>
        /// <returns>The requested team.</returns>
        /// <response code="200">Team retrieved successfully.</response>
        /// <response code="404">Team not found.</response>
        [HttpGet]
        [Authorize(Roles = "Manager,Member")]
        public async Task<ActionResult<TeamDto>> Get()
        {
            var ctx = UserClaimsFactory.From(User);
            return Ok(await _service.GetTeamsByUserCredentialsAsync(ctx));
        }

        /// <summary>
        /// [Admin] Retrieves a specific team by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the team.</param>
        /// <returns>The requested team.</returns>
        /// <response code="200">Team retrieved successfully.</response>
        /// <response code="404">Team not found.</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TeamDto>> GetById(int id)
            => Ok(await _service.GetByIdAsync(id));

        /// <summary>
        /// Creates a new team.
        /// </summary>
        /// <param name="dto">The data required to create the team.</param>
        /// <returns>The newly created team.</returns>
        /// <response code="201">Team created successfully.</response>
        /// <response code="400">Invalid input data.</response>
        [HttpPost]                                       
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<TeamDto>> Create(CreateTeamDto dto)
        {
            var ctx = UserClaimsFactory.From(User);
            var createdTeam = await _service.CreateAsync(dto,ctx);
            return CreatedAtAction(nameof(GetById), new { id = createdTeam.Id }, createdTeam);
        }

        /// <summary>
        /// Updates an existing team.
        /// </summary>
        /// <param name="id">The unique identifier of the team.</param>
        /// <param name="dto">The updated team data.</param>
        /// <returns>The updated team.</returns>
        /// <response code="200">Team updated successfully.</response>
        /// <response code="404">Team not found.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<TeamDto>> Update(int id, UpdateTeamDto dto)           
        {
            var ctx = UserClaimsFactory.From(User);
            return Ok(await _service.UpdateAsync(id, dto, ctx));
        }

        /// <summary>
        /// [Admin] Permanently deletes a team.
        /// </summary>
        /// <param name="id">The unique identifier of the team.</param>
        /// <response code="204">Team deleted successfully.</response>
        /// <response code="404">Team not found.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Soft-deletes a team without permanently removing it.
        /// </summary>
        /// <param name="id">The unique identifier of the team.</param>
        /// <response code="204">Team soft-deleted successfully.</response>
        /// <response code="404">Team not found.</response>
        [HttpPatch("{id}/soft-delete")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var ctx = UserClaimsFactory.From(User);
            await _service.SoftDeleteAsync(id, ctx);
            return NoContent();
        }

        /// <summary>
        /// Restores a previously soft-deleted team.
        /// </summary>
        /// <param name="id">The unique identifier of the team.</param>
        /// <response code="204">Team restored successfully.</response>
        /// <response code="404">Team not found.</response>
        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Restore(int id)
        {
            var ctx = UserClaimsFactory.From(User);
            await _service.RestoreAsync(id, ctx);
            return NoContent();
        }

        /// <summary>
        /// [Admin] Retrieves all soft-deleted teams.
        /// </summary>
        /// <returns>A list of soft-deleted teams.</returns>
        /// <response code="200">Soft-deleted teams retrieved successfully.</response>
        [HttpGet("soft-deleted")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<TeamDto>>> GetDeleted()
            => Ok(await _service.GetAllDeletedTeamsAsync());

        /// <summary>
        /// Retrieves all teams belonging to a specific organization.
        /// </summary>
        /// <param name="organizationId">The unique identifier of the organization.</param>
        /// <returns>A list of teams for the organization.</returns>
        /// <response code="200">Teams retrieved successfully.</response>
        [HttpGet("by-organization/{organizationId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<List<TeamDto>>> GetByOrganization(int organizationId)
        {
            var ctx = UserClaimsFactory.From(User);
            return Ok(await _service.GetTeamsByOrganizationAsync(organizationId,ctx));
        }

        /// <summary>
        /// Retrieves all teams associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of teams for the user.</returns>
        /// <response code="200">Teams retrieved successfully.</response>
        [HttpGet("by-user/{userId}")]
        [Authorize(Roles = "Admin,Manager,TeamLeader,Member")]
        public async Task<ActionResult<List<TeamDto>>> GetByUser(int userId)
        {
            var ctx = UserClaimsFactory.From(User);
            return Ok(await _service.GetTeamsByUserAsync(userId,ctx));
        }
    }

}

