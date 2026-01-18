using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.UserDTOs;

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
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The requested user.</returns>
        /// <response code="200">User retrieved successfully.</response>
        /// <response code="404">User not found.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
            => Ok(await _service.GetByIdAsync(id));

        /// <summary>
        /// Retrieves a specific user by email address.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>The requested user.</returns>
        /// <response code="200">User retrieved successfully.</response>
        /// <response code="404">User not found.</response>
        [HttpGet("by-email")]
        public async Task<ActionResult<UserDto>> GetByEmail([FromQuery] string email)
            => Ok(await _service.GetByEmailAsync(email));

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="dto">The data required to create the user.</param>
        /// <returns>The newly created user.</returns>
        /// <response code="201">User created successfully.</response>
        /// <response code="400">Invalid input data.</response>
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
        {
            var createdUser = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="dto">The updated user data.</param>
        /// <returns>The updated user.</returns>
        /// <response code="200">User updated successfully.</response>
        /// <response code="404">User not found.</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> Update(int id, UpdateUserDto dto)
            => Ok(await _service.UpdateAsync(id, dto));

        /// <summary>
        /// Permanently deletes a user.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <response code="204">User deleted successfully.</response>
        /// <response code="404">User not found.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Soft-deletes a user without permanently removing it.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <response code="204">User soft-deleted successfully.</response>
        /// <response code="404">User not found.</response>
        [HttpPatch("{id}/soft-delete")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _service.SoftDeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Restores a previously soft-deleted user.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <response code="204">User restored successfully.</response>
        /// <response code="404">User not found.</response>
        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            await _service.RestoreAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Retrieves all soft-deleted users.
        /// </summary>
        /// <returns>A list of soft-deleted users.</returns>
        /// <response code="200">Soft-deleted users retrieved successfully.</response>
        [HttpGet("soft-deleted")]
        public async Task<ActionResult<List<UserDto>>> GetDeleted()
            => Ok(await _service.GetAllDeletedUsersAsync());
    }

}

