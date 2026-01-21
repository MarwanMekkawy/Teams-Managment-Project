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
        public IPasswordHasher Hasher { get; }
        public IRefreshTokenRepository RefreshTokenRepo { get; }

        public RefreshTokenService(IConfiguration configuration, IPasswordHasher hasher, IRefreshTokenRepository refreshTokenRepo)
        {
            Configuration = configuration;
            Hasher = hasher;
            RefreshTokenRepo = refreshTokenRepo;
        }


        private string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes);
        }

        public async Task<RefreshTokenEntity> CreateAndStoreRefreshTokenAsync(int userId)
        {
            var refreshToken = new RefreshTokenEntity();
            refreshToken.CreatedAt = DateTime.UtcNow;
            refreshToken.ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(Configuration["RefreshToken:ExpiryInMinutes"]!));
            refreshToken.UserId = userId;
            var tokenBytes = GenerateRefreshToken();
            refreshToken.TokenHash = Hasher.Hash(tokenBytes);
            RefreshTokenRepo.AddToken(refreshToken);

            await RefreshTokenRepo.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<(bool IsValid, RefreshTokenEntity? Token)> ValidateRefreshTokenAsync(string refreshToken)
        {
            var hash = Hasher.Hash(refreshToken);
            var token = await RefreshTokenRepo.GetActiveByHashAsync(hash);
            if (token == null) return (false, null);
            return (true, token);
        }

        public async Task<RefreshTokenEntity?> RotateRefreshTokenAsync(string refreshToken, int userId)
        {
            var hash = Hasher.Hash(refreshToken);
            var existingToken = await RefreshTokenRepo.GetByHashAsync(hash);

            if (existingToken == null)  return null;

            // check if refresh token used is valid 
            if (!existingToken.IsActive)
            {
                // check if refresh token used was replaced before
                if (existingToken.ReplacedByTokenHash != null)
                {
                    await RefreshTokenRepo.RevokeAllUserTokensAsync(userId);
                    await RefreshTokenRepo.SaveChangesAsync();
                }
                return null;
            }

            var newRawToken = GenerateRefreshToken();
            var newTokenHash = Hasher.Hash(newRawToken);
            var newToken = new RefreshTokenEntity
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(Configuration["RefreshToken:ExpiryInMinutes"]!)),
                TokenHash = newTokenHash
            };

            RefreshTokenRepo.AddToken(newToken);

            await RefreshTokenRepo.RotateTokenAsync(existingToken.TokenHash, newTokenHash);
            await RefreshTokenRepo.SaveChangesAsync();

            return newToken;
        }

        public async Task RevokeAllUserRefreshTokensAsync(int userId)
        {
            await RefreshTokenRepo.RevokeAllUserTokensAsync(userId);
            await RefreshTokenRepo.SaveChangesAsync();
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            refreshToken = Hasher.Hash(refreshToken);
            await RefreshTokenRepo.RevokeTokenAsync(refreshToken);
            await RefreshTokenRepo.SaveChangesAsync();
        }
    }
}
