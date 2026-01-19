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
        Task<(bool Success, string? Token)> LoginAsync(LoginDto dto);
        Task<(bool Success, string? Token)> RegisterAsync(RegisterDto dto);
        Task ChangePasswordAsync(int userId, string oldPassword, string newPassword);
    }
}
