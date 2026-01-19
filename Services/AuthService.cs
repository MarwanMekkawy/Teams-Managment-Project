using AutoMapper;
using Domain.Contracts;
using Domain.Contracts.Security;
using Domain.Entities;
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
        private readonly ITokenService token;
        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher hasher, ITokenService token)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.hasher = hasher;
            this.token = token;
        }

        public async Task<(bool Success, string? Token)> RegisterAsync(RegisterDto dto)
        {
            // Check if email format is valid
            var normalizedEmail = NormalizeEmail(dto.Email);
            if (!IsValidEmail(normalizedEmail))
                throw new ArgumentException("Invalid email format");

            // Check if user already exists 
            var existingEmail = await unitOfWork.users.GetByEmailAsync(normalizedEmail);
            if (existingEmail != null) throw new ArgumentException("Email is already in use");

            var newPassword = dto.Password;

            // Validate password length
            if (newPassword.Length < 8)
                throw new ArgumentException("New password must be at least 8 characters");

            // Validate password strength
            if (!IsStrongPassword(newPassword))
                throw new ArgumentException("Password is too weak");


            var newUser = mapper.Map<User>(dto);
            newUser.Email = normalizedEmail; 
            newUser.PasswordHash = hasher.Hash(newPassword);

            // Save user to database
            unitOfWork.users.Add(newUser);
            await unitOfWork.SaveChangesAsync();

            // Generate JWT token after user has ID
            var newToken = token.CreateToken(newUser);

            return (true, newToken);
        }

        public async Task<(bool Success, string? Token)> LoginAsync(LoginDto dto)
        {
            // Normalize email for consistent lookup
            var normalizedEmail = NormalizeEmail(dto.Email);

            // Get user from database
            var existingUser = await unitOfWork.users.GetByEmailAsync(normalizedEmail);

            // Use dummy hash if user doesn't exist
            var existingPwHash = existingUser?.PasswordHash ?? "$2a$11$N9qo8uLOickgx2ZMRZoMye.ML3LQeKjFYGGZQDI6RgL3t4SZPp/5a";

            // Verify password against hash
            var isPasswordValid = hasher.Verify(existingPwHash, dto.Password);

            // Check if login is valid
            var isValidLogin = existingUser != null && isPasswordValid;
            if (!isValidLogin) throw new ArgumentException($"Wrong password or Email");

            // JWT token
            var newToken = token.CreateToken(existingUser!);

            return (true, newToken);
        }

        public async Task ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            // Validate old password input
            if (string.IsNullOrWhiteSpace(oldPassword))
                throw new ArgumentException("Old password is required", nameof(oldPassword));

            // Validate new password input
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("New password is required", nameof(newPassword));

            // Get user by ID
            var existingUser = await unitOfWork.users.GetAsync(userId);
            if (existingUser == null)
                throw new ArgumentException("User not found", nameof(userId));

            // Verify current password
            var isOldPasswordValid = hasher.Verify(existingUser.PasswordHash, oldPassword);
            if (!isOldPasswordValid)
                throw new ArgumentException("Current password is incorrect");

            // Check if new password is different from old
            if (oldPassword == newPassword)
                throw new ArgumentException("New password cannot be the same as old password");

            // Validate new password length
            if (newPassword.Length < 8)
                throw new ArgumentException("New password must be at least 8 characters");

            // Validate new password strength
            if (!IsStrongPassword(newPassword))
                throw new ArgumentException("Password is too weak");


            existingUser.PasswordHash = hasher.Hash(newPassword);

            // Save changes 
            unitOfWork.users.Update(existingUser);
            await unitOfWork.SaveChangesAsync();
        }

        // Check if password strong
        private bool IsStrongPassword(string password)
        {
            return password.Length >= 8 && password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) && password.Any(char.IsDigit);
        }

        // Normalize email 
        private string NormalizeEmail(string email)
        {
            return email?.Trim().ToLowerInvariant() ?? string.Empty;
        }

        // Validate email 
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))return false;
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
    }
}
