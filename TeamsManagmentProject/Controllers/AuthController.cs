using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Abstractions;
using Services.Abstractions.Security;
using Services.RefreshToken;
using Shared.AuthDTOs;
using System.Security.Claims;

namespace TeamsManagmentProject.API.Controllers
{
    /// <summary>
    /// Handles authentication and authorization related operations.
    /// </summary>
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        /// <summary>
        /// Registers a new user and issues a JWT token.
        /// Refresh token is stored in an HttpOnly cookie for security.
        /// </summary>
        /// <param name="dto">User registration data.</param>
        /// <returns>JWT access token.</returns>
        /// <response code="200">User registered successfully.</response>
        /// <response code="400">Invalid input or email already exists.</response>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await authService.RegisterAsync(dto);

            AppendRefreshTokenCookie(result.RefreshToken!);

            return Ok(new { jwtToken = result.JwtToken });
        }

        /// <summary>
        /// Authenticates a user and issues a JWT token.
        /// Refresh token is stored securely in an HttpOnly cookie.
        /// </summary>
        /// <param name="dto">User login credentials.</param>
        /// <returns>JWT access token.</returns>
        /// <response code="200">Login successful.</response>
        /// <response code="401">Invalid email or password.</response>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await authService.LoginAsync(dto);

            AppendRefreshTokenCookie(result.RefreshToken!);

            return Ok(new { jwtToken = result.JwtToken });
        }

        /// <summary>
        /// Logs out the user by revoking the refresh token.
        /// </summary>
        /// <returns>No content if logout succeeds.</returns>
        /// <response code="204">Logout successful.</response>
        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrWhiteSpace(refreshToken)) return NoContent();

            await authService.LogoutAsync(refreshToken);

            DeleteRefreshTokenCookie();

            return NoContent();
        }

        /// <summary>
        /// Refreshes the user session using a refresh token.
        /// </summary>
        /// <returns>New JWT access token.</returns>
        /// <response code="200">Session refreshed successfully.</response>
        /// <response code="401">Invalid or expired refresh token.</response>
        /// <response code="403">Refresh token reuse detected.</response>
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrWhiteSpace(refreshToken)) return Unauthorized();

            var result = await authService.RefreshSessionAsync(refreshToken);

            AppendRefreshTokenCookie(result.RefreshToken!);

            return Ok(new { jwtToken = result.JwtToken });
        }

        /// <summary>
        /// Changes the password of the currently authenticated user.
        /// </summary>
        /// <param name="dto">Old and new password data.</param>
        /// <returns>No content if password is changed successfully.</returns>
        /// <response code="204">Password changed successfully.</response>
        /// <response code="400">Validation failure.</response>
        /// <response code="401">User is not authenticated.</response>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmNewPassword) return BadRequest("Passwords do not match");
            
            if (! int.TryParse(User.FindFirstValue("id"), out var userId)) return Unauthorized();

            await authService.ChangePasswordAsync(userId, dto.OldPassword, dto.NewPassword);

            return NoContent();
        }

        // =========================
        // Cookie helper methods
        // =========================

        // Stores the refresh token in HttpOnly cookie
        private void AppendRefreshTokenCookie(string refreshToken)
        {
            Response.Cookies.Append("refreshToken",refreshToken,
                new CookieOptions
                {
                    HttpOnly = true,        
                    Secure = true,          
                    SameSite = SameSiteMode.Strict,
                    Path = "/api/v1/auth" 
                });
        }

        // Removes the refresh token cookie
        private void DeleteRefreshTokenCookie()
        {
            Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/api/v1/auth" });
        }
    }
}