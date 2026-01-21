using Shared.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IAuthService
    {
        Task<(bool Success, string? JwtToken, string? RefreshToken)> RegisterAsync(RegisterDto dto);
        Task<(bool Success, string? JwtToken, string? RefreshToken)> LoginAsync(LoginDto dto);
        Task ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task LogoutAsync(string refreshToken);
        Task<(bool Success, string? JwtToken, string? RefreshToken)> RefreshSession(string refreshToken);
    }
}
