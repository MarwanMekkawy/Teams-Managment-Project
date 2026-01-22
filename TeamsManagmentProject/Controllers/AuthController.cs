using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Abstractions;
using Services.Abstractions.Security;
using Services.RefreshToken;
using Shared.AuthDTOs;

namespace TeamsManagmentProject.API.Controllers
{
    /// <summary>
    /// Handles authentication and authorization related operations.
    /// </summary>
    [Route("api/v1/Auth")]
    [ApiController]
    public class AuthController(IAuthService authService, IRefreshTokenService refreshTokenService) : ControllerBase
    {
        /// <summary>
        /// Registers a new user and returns a JWT token upon successful registration.
        /// </summary>
        /// <param name="dto">User registration data.</param>
        /// <returns>JWT token if registration succeeds.</returns>
        /// <response code="200">User registered successfully.</response>
        /// <response code="400">Invalid input or email already exists.</response>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await authService.RegisterAsync(dto);
            var refreshTokenEntity = await refreshTokenService.CreateAndStoreRefreshTokenAsync();
            return Ok(new{success = result.Success, token = result.Token});
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="dto">User login credentials.</param>
        /// <returns>JWT token if login succeeds.</returns>
        /// <response code="200">Login successful.</response>
        /// <response code="400">Invalid email or password.</response>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await authService.LoginAsync(dto);
            return Ok(new { success = result.Success, token = result.Token });
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>No content if logout succeeds.</returns>
        /// <response code="204">Logout successful.</response>
        /// <response code="401">User is not authenticated.</response>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> logout(string refreshToken)
        {
            return NoContent();
        }

        /// <summary>
        /// Refreshes the user session.
        /// </summary>
        /// <returns>New JWT and refresh token.</returns>
        /// <response code="200">Session refreshed successfully.</response>
        /// <response code="401">Invalid or expired refresh token.</response>
        /// <response code="403">Refresh token reuse detected.</response>
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        { 

        }

            /// <summary>
            /// Changes the password of the currently authenticated user.
            /// </summary>
            /// <param name="dto">Old and new password data.</param>
            /// <returns>No content if password is changed successfully.</returns>
            /// <response code="204">Password changed successfully.</response>
            /// <response code="400">Invalid password or validation failure.</response>
            /// <response code="401">User is not authenticated.</response>
            [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userId = int.Parse(User.FindFirst("id")!.Value);
            await authService.ChangePasswordAsync(userId, dto.OldPassword, dto.NewPassword);
            return NoContent();
        }
    }
}
