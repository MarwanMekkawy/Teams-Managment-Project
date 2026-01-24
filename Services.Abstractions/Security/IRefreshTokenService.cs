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
        Task<(RefreshTokenEntity StoredToken, string PlaintextToken)> CreateAndStoreRefreshTokenAsync(int userId);
        Task<RefreshTokenEntity?> ValidateRefreshTokenAsync(string refreshToken);
        Task<(RefreshTokenEntity StoredToken, string PlaintextToken)?> RotateRefreshTokenAsync(string refreshToken, int userId);
        Task RevokeRefreshTokenAsync(string refreshToken);
        Task RevokeAllUserRefreshTokensAsync(int userId);
    }
}
