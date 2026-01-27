using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.TaskDTOs;
using TeamsManagmentProject.API.ApiClaimsFactory;

namespace TeamsManagmentProject.API.Controllers
{
    /// <summary>
    /// Exposes endpoints for managing tasks and assignments.
    /// </summary>
    [ApiController]
    [Route("api/v1/tasks")]
    public class TasksController(ITaskService _service) : ControllerBase
    {
        /// <summary>
        /// Retrieves a specific task by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the task.</param>
        /// <returns>The requested task.</returns>
        /// <response code="200">Task retrieved successfully.</response>
        /// <response code="404">Task not found.</response>
        [HttpGet]
        [Authorize(Roles = "Admin,Manager,TeamLeader,Member")]
        public async Task<ActionResult<TaskDto>> GetById(int id)
        {
            var ctx = UserClaimsFactory.From(User);
            return Ok(await _service.GetByIdAsync(id, ctx));
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="dto">The data required to create the task.</param>
        /// <returns>The newly created task.</returns>
        /// <response code="201">Task created successfully.</response>
        /// <response code="400">Invalid input data.</response>
        [HttpPost]
        [Authorize(Roles = "Admin,Manager,TeamLeader")]
        public async Task<ActionResult<TaskDto>> Create(CreateTaskDto dto)
        {
            var ctx = UserClaimsFactory.From(User);
            var createdTask = await _service.CreateAsync(dto, ctx);
            return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">The unique identifier of the task.</param>
        /// <param name="dto">The updated task data.</param>
        /// <returns>The updated task.</returns>
        /// <response code="200">Task updated successfully.</response>
        /// <response code="404">Task not found.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager,TeamLeader")]
        public async Task<ActionResult<TaskDto>> Update(int id, UpdateTaskDto dto)
        {
            var ctx = UserClaimsFactory.From(User);
            return Ok(await _service.UpdateAsync(id, dto, ctx));
        }

        /// <summary>
        /// Permanently deletes a task.
        /// </summary>
        /// <param name="id">The unique identifier of the task.</param>
        /// <response code="204">Task deleted successfully.</response>
        /// <response code="404">Task not found.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var ctx = UserClaimsFactory.From(User);
            await _service.DeleteAsync(id, ctx);
            return NoContent();
        }

        /// <summary>
        /// Soft-deletes a task without permanently removing it.
        /// </summary>
        /// <param name="id">The unique identifier of the task.</param>
        /// <response code="204">Task soft-deleted successfully.</response>
        /// <response code="404">Task not found.</response>
        [HttpPatch("{id}/soft-delete")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var ctx = UserClaimsFactory.From(User);
            await _service.SoftDeleteAsync(id, ctx);
            return NoContent();
        }

        /// <summary>
        /// Restores a previously soft-deleted task.
        /// </summary>
        /// <param name="id">The unique identifier of the task.</param>
        /// <response code="204">Task restored successfully.</response>
        /// <response code="404">Task not found.</response>
        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Restore(int id)
        {
            var ctx = UserClaimsFactory.From(User);
            await _service.RestoreAsync(id, ctx);
            return NoContent();
        }

        /// <summary>
        /// Retrieves all soft-deleted tasks.
        /// </summary>
        /// <returns>A list of soft-deleted tasks.</returns>
        /// <response code="200">Soft-deleted tasks retrieved successfully.</response>
        [HttpGet("soft-deleted")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<List<TaskDto>>> GetDeleted()
        {
            var ctx = UserClaimsFactory.From(User);
            return Ok(await _service.GetAllDeletedTasksAsync(ctx));
        }

        /// <summary>
        /// Changes the status of a task.
        /// </summary>
        /// <param name="id">The unique identifier of the task.</param>
        /// <param name="status">The new status to apply.</param>
        /// <response code="204">Task status updated successfully.</response>
        /// <response code="404">Task not found.</response>
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,Manager,TeamLeader,Member")]
        public async Task<IActionResult> ChangeStatus(int id, TaskEntityStatus? status)
        {
            var ctx = UserClaimsFactory.From(User);
            await _service.ChangeStatusAsync(id, status, ctx);
            return NoContent();
        }

        /// <summary>
        /// Assigns a task to a specific user.
        /// </summary>
        /// <param name="id">The unique identifier of the task.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <response code="204">Task assigned successfully.</response>
        /// <response code="404">Task or user not found.</response>
        [HttpPatch("{id}/assign/{userId}")]
        [Authorize(Roles = "Admin,Manager,TeamLeader")]
        public async Task<IActionResult> Assign(int id, int userId)
        {
            var ctx = UserClaimsFactory.From(User);
            await _service.AssignToUserAsync(id, userId, ctx);
            return NoContent();
        }
    }

}
