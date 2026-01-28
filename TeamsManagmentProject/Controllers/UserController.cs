using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.UserDTOs;
using TeamsManagmentProject.API.ApiClaimsFactory;

namespace TeamsManagmentProject.API.Controllers
{
    /// <summary>
    /// Exposes endpoints for managing users.
    /// </summary>
    [ApiController]
    [Route("api/v1/users")]
    public class UserController(IUserService _service) : ControllerBase
    {
        /// <summary>
        /// Retrieves a specific user by its identifier.
        /// Managers and TeamLeaders can only access users in their organization.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <response code="200">User retrieved successfully.</response>
        /// <response code="404">User not found.</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager,TeamLeader")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var ctx = UserClaimsFactory.From(User);
            return Ok(await _service.GetByIdAsync(id, ctx));
        }

        /// <summary>
        /// Retrieves a specific user by email address.
        /// Managers and TeamLeaders can only access users in their organization.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <response code="200">User retrieved successfully.</response>
        /// <response code="404">User not found.</response>
        [HttpGet("by-email")]
        [Authorize(Roles = "Admin,Manager,TeamLeader")]
        public async Task<ActionResult<UserDto>> GetByEmail([FromQuery] string email)
        {
            var ctx = UserClaimsFactory.From(User);
            return Ok(await _service.GetByEmailAsync(email, ctx));
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="dto">User creation data.</param>
        /// <response code="201">User created successfully.</response>
        /// <response code="400">Invalid input data.</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
        {
            var createdUser = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <param name="dto">Updated user data.</param>
        /// <response code="200">User updated successfully.</response>
        /// <response code="404">User not found.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> Update(int id, UpdateUserDto dto)
            => Ok(await _service.UpdateAsync(id, dto));

        /// <summary>
        /// Permanently deletes a user.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <response code="204">User deleted successfully.</response>
        /// <response code="404">User not found.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Soft-deletes a user without permanently removing it.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <response code="204">User soft-deleted successfully.</response>
        /// <response code="404">User not found.</response>
        [HttpPatch("{id}/soft-delete")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var ctx = UserClaimsFactory.From(User);
            await _service.SoftDeleteAsync(id, ctx);
            return NoContent();
        }

        /// <summary>
        /// Restores a previously soft-deleted user.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <response code="204">User restored successfully.</response>
        /// <response code="404">User not found.</response>
        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Restore(int id)
        {
            var ctx = UserClaimsFactory.From(User);
            await _service.RestoreAsync(id, ctx);
            return NoContent();
        }

        /// <summary>
        /// Retrieves all soft-deleted users.[paginated]
        /// </summary>
        /// <response code="200">Soft-deleted users retrieved successfully.</response>
        [HttpGet("soft-deleted")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<List<UserDto>>> GetDeleted(int pageNumber = 1, int pageSize = 10)
        {
            var ctx = UserClaimsFactory.From(User);
            return Ok(await _service.GetAllDeletedUsersAsync(pageNumber, pageSize, ctx));
        }
    }
}

