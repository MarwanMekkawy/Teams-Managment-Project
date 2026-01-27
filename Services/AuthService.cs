using AutoMapper;
using Domain.Contracts;
using Domain.Contracts.Security;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.IdentityModel.JsonWebTokens;
using Services.Abstractions;
using Services.Abstractions.Security;
using Shared.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IPasswordHasher hasher;
        private readonly IJwtTokenService token;
        private readonly IRefreshTokenService refreshToken;
        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher hasher, IJwtTokenService token, IRefreshTokenService refreshToken)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.hasher = hasher;
            this.token = token;
            this.refreshToken = refreshToken;
        }

        // Check if password strong [helper]
        private bool IsStrongPassword(string password)
        {
            return password.Length >= 8 && password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) && password.Any(char.IsDigit);
        }

        // Normalize email [helper]
        private string NormalizeEmail(string email)
        {
            return email?.Trim().ToLowerInvariant() ?? string.Empty;
        }

        // Validate email [helper]
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        public async Task<(string? JwtToken, string? RefreshToken)> RegisterAsync(RegisterDto dto)
        {
            // Check if email format is valid
            var normalizedEmail = NormalizeEmail(dto.Email);
            if (!IsValidEmail(normalizedEmail)) throw new BadRequestException("Invalid email format");

            // Check if user already exists 
            var existingEmail = await unitOfWork.users.GetByEmailAsync(normalizedEmail);
            if (existingEmail != null) throw new ConflictException("Email is already in use");

            // PW validation //

            // Check for null or empty
            if (string.IsNullOrWhiteSpace(dto.Password)) throw new BadRequestException("Password Cant be empty");
            // Check if confirm pw & pw match 
            if (dto.ConfirmPassword != dto.Password) throw new BadRequestException("New password confirmation must match the password");

            var newPassword = dto.Password;
            // Validate password length
            if (newPassword.Length < 8) throw new BadRequestException("New password must be at least 8 characters");
            // Validate password strength
            if (!IsStrongPassword(newPassword)) throw new BadRequestException("Password is too weak");


            var newUser = mapper.Map<User>(dto);
            newUser.Email = normalizedEmail; 
            newUser.PasswordHash = hasher.Hash(newPassword);

            // Save user to database
            unitOfWork.users.Add(newUser);
            await unitOfWork.SaveChangesAsync();

            // Generate JWT token after user has ID
            var newJwtToken = token.CreateToken(newUser);

            // Generate Refresh token
            var newRefreshToken = await refreshToken.CreateAndStoreRefreshTokenAsync(newUser.Id);

            return (newJwtToken, newRefreshToken.PlaintextToken);
        }

        public async Task<(string? JwtToken, string? RefreshToken)> LoginAsync(LoginDto dto)
        {
            // Normalize email for consistent lookup
            var normalizedEmail = NormalizeEmail(dto.Email);

            // Get user from database
            var existingUser = await unitOfWork.users.GetByEmailAsync(normalizedEmail);

            // Use dummy hash if user doesn't exist
            var existingPwHash = existingUser?.PasswordHash ?? "AQAAAAIAAYagAAAAEAAAAAAAAAAAAAAAAAAAAABIN8F2glsG2w0ThRc6b//V2SgXfV/+/2ZFaUf66RukGA==";

            // Verify password against hash
            var isPasswordValid = hasher.Verify(existingPwHash, dto.Password);

            // Check if login is valid
            var isValidLogin = existingUser != null && isPasswordValid;
            if (!isValidLogin) throw new UnauthorizedException($"Wrong password or Email");

            // JWT token
            var newToken = token.CreateToken(existingUser!);
            // refresh token
            var newRefreshToken = await refreshToken.CreateAndStoreRefreshTokenAsync(existingUser!.Id);

            return (newToken, newRefreshToken.PlaintextToken);
        }

        public async Task ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            // Validate old password input
            if (string.IsNullOrWhiteSpace(oldPassword)) throw new BadRequestException("Old password is required");

            // Validate new password input
            if (string.IsNullOrWhiteSpace(newPassword)) throw new BadRequestException("New password is required");

            // Get user by ID
            var existingUser = await unitOfWork.users.GetAsync(userId);
            if (existingUser == null) throw new UnauthorizedException("User not found");

            // Verify current password
            var isOldPasswordValid = hasher.Verify(existingUser.PasswordHash, oldPassword);
            if (!isOldPasswordValid) throw new UnauthorizedException("Current password is incorrect");

            // Check if new password is different from old
            if (oldPassword == newPassword) throw new BadRequestException("New password cannot be the same as old password");          

            // Validate new password length
            if (newPassword.Length < 8) throw new BadRequestException("New password must be at least 8 characters");

            // Validate new password strength
            if (!IsStrongPassword(newPassword)) throw new BadRequestException("Password is too weak");


            existingUser.PasswordHash = hasher.Hash(newPassword);

            // Save changes 
            unitOfWork.users.Update(existingUser);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task LogoutAsync(string plainRefreshToken)
        {
           await refreshToken.RevokeRefreshTokenAsync(plainRefreshToken);
        }
        
        public async Task<(string? JwtToken, string? RefreshToken)> RefreshSessionAsync(string plainrefreshToken)
        {
            // get token entity
            var existingRefreshToken = await refreshToken.ValidateRefreshTokenAsync(plainrefreshToken);

            if (existingRefreshToken == null) throw new UnauthorizedException("Invalid refresh token"); 

            // rotate token and creating new one
            var rotatedNewRefreshToken = await refreshToken.RotateRefreshTokenAsync(plainrefreshToken, existingRefreshToken.UserId);

            if (rotatedNewRefreshToken == null) throw new ConflictException("Refresh token reuse detected"); 

            // load user to issue new JWT
            var user = await unitOfWork.users.GetAsync(existingRefreshToken.UserId);
            if (user == null) throw new NotFoundException("User not found");

            // creating new jwt
            var newJwt = token.CreateToken(user);

            return (newJwt, rotatedNewRefreshToken.Value.PlaintextToken);

        }
    }

}
