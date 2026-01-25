using Domain.Contracts;
using Domain.Contracts.IRefreshTokens;
using Domain.Contracts.Security;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Services.Abstractions.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.RefreshToken
{
    public class RefreshTokenService : IRefreshTokenService
    {
        public IConfiguration Configuration { get; }
        public IRefreshTokenRepository RefreshTokenRepo { get; }

        public RefreshTokenService(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepo)
        {
            Configuration = configuration;
            RefreshTokenRepo = refreshTokenRepo;
        }

        // generate token byets [helper]
        private string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes);
        }
        // hash tokens [helper]
        private string HashRefreshToken(string token)
        {
            var secret = Configuration["RefreshToken:Secret"]!;
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(token)));
        }

        // create token
        public async Task<(RefreshTokenEntity StoredToken, string PlaintextToken)>CreateAndStoreRefreshTokenAsync(int userId)
        {
            var rawToken = GenerateRefreshToken();

            var refreshToken = new RefreshTokenEntity
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(Configuration["RefreshToken:ExpiryInMinutes"]!)),
                TokenHash = HashRefreshToken(rawToken)
            };

            RefreshTokenRepo.AddToken(refreshToken);
            await RefreshTokenRepo.SaveChangesAsync();
            
            return (refreshToken, rawToken);
        }       

        // validat token
        public async Task<RefreshTokenEntity?> ValidateRefreshTokenAsync(string refreshToken)
        {
            var hash = HashRefreshToken(refreshToken);
            return await RefreshTokenRepo.GetActiveByHashAsync(hash);
        }

        //rotate token
        public async Task<(RefreshTokenEntity StoredToken, string PlaintextToken)?>RotateRefreshTokenAsync(string refreshToken, int userId)
        {
            var hash = HashRefreshToken(refreshToken);
            var existingToken = await RefreshTokenRepo.GetByHashAsync(hash);

            if (existingToken == null)
                return null;

            // reuse invalid token [not Atctive token - old token that is replaced]
            if (!existingToken.IsActive)
            {
                if (existingToken.ReplacedByTokenHash != null)
                {
                    // the use of old token that is replaced
                    await RefreshTokenRepo.RevokeAllUserTokensAsync(userId);
                    await RefreshTokenRepo.SaveChangesAsync();
                }
                return null;
            }

            var newRawToken = GenerateRefreshToken();
            var newHash = HashRefreshToken(newRawToken);

            var newToken = new RefreshTokenEntity
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(Configuration["RefreshToken:ExpiryInMinutes"]!)),
                TokenHash = newHash
            };

            // revoke and replace old token
            existingToken.RevokedAt = DateTime.UtcNow;
            existingToken.ReplacedByTokenHash = newHash;

            RefreshTokenRepo.AddToken(newToken);
            await RefreshTokenRepo.SaveChangesAsync();

            return (newToken, newRawToken);
        }
         
        //revoke all toekns
        public async Task RevokeAllUserRefreshTokensAsync(int userId)
        {
            await RefreshTokenRepo.RevokeAllUserTokensAsync(userId);
            await RefreshTokenRepo.SaveChangesAsync();
        }

        //revoke 1 token
        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var hashedRefreshToken = HashRefreshToken(refreshToken);
            await RefreshTokenRepo.RevokeTokenAsync(hashedRefreshToken);
            await RefreshTokenRepo.SaveChangesAsync();
        }
    }
}
