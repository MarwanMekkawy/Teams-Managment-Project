using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.TeamMemberDTOs;
using TeamsManagmentProject.API.ApiClaimsFactory;

namespace TeamsManagmentProject.API.Controllers
{
    /// <summary>
    /// Exposes endpoints for managing team membership.
    /// </summary>
    [ApiController]
    [Route("api/v1/teams/{teamId}/members")]
    public class TeamMembersController(ITeamMemberService _service) : ControllerBase
    {
        /// <summary>
        /// Adds a user to a team.
        /// </summary>
        /// <param name="dto">Team member creation data.</param>
        /// <response code="201">Team member added successfully.</response>
        /// <response code="400">Invalid input data.</response>
        [HttpPost]
        [Authorize(Roles = "Admin,Manager,TeamLeader")]
        public async Task<IActionResult> Add(int teamId, CreateTeamMemberDto dto)
        {
            var ctx = UserClaimsFactory.From(User);
            await _service.AddMemberAsync(teamId, dto, ctx);
            return Created($"/api/v1/teams/{teamId}/members/{dto.UserId}",null);
        }

        /// <summary>
        /// Removes a user from a team.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <response code="204">Team member removed successfully.</response>
        /// <response code="404">Team or user not found.</response>
        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin,Manager,TeamLeader")]
        public async Task<IActionResult> Remove(int teamId, int userId)
        {
            var ctx = UserClaimsFactory.From(User);
            await _service.RemoveMemberAsync(teamId, userId, ctx);
            return NoContent();
        }

        /// <summary>
        /// Checks whether a user is a member of a team.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <response code="200">Membership status retrieved successfully.</response>
        [HttpGet("{userId}/exists")]
        [Authorize(Roles = "Admin,Manager,TeamLeader")]
        public async Task<ActionResult<bool>> CheckMembership(int teamId, int userId)
        {
            var ctx = UserClaimsFactory.From(User);
            return Ok(await _service.IsMemberAsync(teamId, userId, ctx));
        }
    }
}
