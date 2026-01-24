using Domain.Contracts.IRefreshTokens;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.RefreshTokens
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;
        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RefreshTokenEntity>> GetActiveTokensByUserIdAsync(int userId)
        {
            return await _context.RefreshTokens.Where(t => t.UserId == userId && t.RevokedAt == null && t.ExpiresAt > DateTime.UtcNow).ToListAsync();
        }
        public async Task<RefreshTokenEntity?> GetActiveByHashAsync(string tokenHash)
        {
            return await _context.RefreshTokens.Where(t => t.TokenHash == tokenHash && t.RevokedAt == null && t.ExpiresAt > DateTime.UtcNow).FirstOrDefaultAsync();
        }

        public async Task<RefreshTokenEntity?> GetByHashAsync(string tokenHash)
        {
            return await _context.RefreshTokens.Where(t => t.TokenHash == tokenHash).FirstOrDefaultAsync();
        }

        public async Task RevokeAllUserTokensAsync(int userId)
        {
            var time = DateTime.UtcNow;
            var activeTokens = await _context.RefreshTokens.Where(t => t.UserId == userId && t.RevokedAt == null && t.ExpiresAt > time).ToListAsync();
            foreach (var t in activeTokens)
            {
                t.RevokedAt = time;
            }
        }

        public async Task RevokeTokenAsync(string tokenHash, string? replacedByHash = null)
        {
            var token = await _context.RefreshTokens.Where(t => t.TokenHash == tokenHash).FirstOrDefaultAsync();
            if (token == null) return;
            token.RevokedAt = DateTime.UtcNow;
            token.ReplacedByTokenHash = replacedByHash;
        }

        public async Task RotateTokenAsync(string oldTokenHash, string newTokenHash)
        {
            var token = await _context.RefreshTokens .FirstOrDefaultAsync(t => t.TokenHash == oldTokenHash);
            if (token == null) return;
            token.RevokedAt = DateTime.UtcNow;
            token.ReplacedByTokenHash = newTokenHash;
        }

        public void AddToken(RefreshTokenEntity refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
