using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Security
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenEntity> CreateAndStoreRefreshTokenAsync(int userId);
        Task<(bool IsValid, RefreshTokenEntity? Token)> ValidateRefreshTokenAsync(string refreshToken);
        Task<RefreshTokenEntity?> RotateRefreshTokenAsync(string refreshToken, int userId);
        Task RevokeRefreshTokenAsync(string refreshToken);
        Task RevokeAllUserRefreshTokensAsync(int userId);
    }
}
