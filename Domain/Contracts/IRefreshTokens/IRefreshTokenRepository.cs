using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.IRefreshTokens
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshTokenEntity?> GetByHashAsync(string tokenHash);
        Task<IEnumerable<RefreshTokenEntity>> GetActiveTokensByUserIdAsync(int userId);
        Task<RefreshTokenEntity?> GetActiveByHashAsync(string tokenHash);
        Task RevokeTokenAsync(string tokenHash, string? replacedByHash = null);
        Task RevokeAllUserTokensAsync(int userId);
        Task RotateTokenAsync(string oldTokenHash, string newTokenHash);
        void AddToken(RefreshTokenEntity refreshToken);
        Task<int> SaveChangesAsync();
    }
}
