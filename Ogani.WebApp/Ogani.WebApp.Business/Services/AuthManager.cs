using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Ogani.WebApp.Business.Authentication;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.Auth;
using Ogani.WebApp.Entities.Identity;
using System.Text;

namespace Ogani.WebApp.Business.Services
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IValidator<RegisterDTO> _registerValidator;
        private readonly IValidator<LoginDTO> _loginValidator;
        private readonly IValidator<ProfileUpdateDTO> _profileUpdateValidator;
        private readonly IValidator<PasswordChangeDTO> _passwordChangeValidator;
        private readonly IValidator<ForgotPasswordDTO> _forgotPasswordValidator;
        private readonly IValidator<ResetPasswordDTO> _resetPasswordValidator;
        private readonly IEmailService _emailService;

        public AuthManager(UserManager<AppUser> userManager, IValidator<RegisterDTO> registerValidator, IValidator<LoginDTO> loginValidator, IEmailService emailService, IValidator<ProfileUpdateDTO> profileUpdateValidator, IValidator<PasswordChangeDTO> passwordChangeValidator, IValidator<ForgotPasswordDTO> forgotPasswordValidator, IValidator<ResetPasswordDTO> resetPasswordValidator)
        {
            _userManager = userManager;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _emailService = emailService;
            _profileUpdateValidator = profileUpdateValidator;
            _passwordChangeValidator = passwordChangeValidator;
            _forgotPasswordValidator = forgotPasswordValidator;
            _resetPasswordValidator = resetPasswordValidator;
        }

        public async Task<AppUserDTO> RegisterAsync(RegisterDTO dto)
        {
            var valiadtionResult = await _registerValidator.ValidateAsync(dto);
            if (!valiadtionResult.IsValid)
                throw new BusinessValidationException(valiadtionResult.Errors);

            bool isExsist = await _userManager.Users.AnyAsync(u => u.UserName == dto.UserName);
            if (isExsist)
                throw new BusinessValidationException("username already exsist");

            var user = new AppUser
            {
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => new ValidationFailure(string.Empty, error.Description) { ErrorCode = error.Code }).ToList();
                throw new BusinessValidationException(errors);
            }


            await _userManager.AddToRoleAsync(user, "Member");

            return new AppUserDTO
            {
                Id = user.Id,
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Roles = new() { "Member" }
            };
        }

        public async Task<AppUserDTO> ValidateUserAsync(LoginDTO dto)
        {
            var validationResult = await _loginValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);

            AppUser? user = null;

            if (dto.UserNameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(dto.UserNameOrEmail);
            }

            user ??= await _userManager.FindByNameAsync(dto.UserNameOrEmail)
                 ?? throw new BusinessValidationException("Username or password is false.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordValid)
                throw new BusinessValidationException("Username or password is false.");


            var userRoles = await _userManager.GetRolesAsync(user);

            return new AppUserDTO
            {
                Id = user.Id,
                UserName = user.UserName!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                IsEmailConfirmed = user.EmailConfirmed,
                Roles = userRoles.ToList() ?? []
            };
        }

        public async Task<ProfileUpdateDTO> GetUserProfileAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id) ??
                throw new NotFoundException("User not found.");

            return new ProfileUpdateDTO
            {
                Id = user.Id,
                UserName = user.UserName!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!
            };
        }

        public async Task UpdateUserProfileAsync(ProfileUpdateDTO dto)
        {
            var validationResult = await _profileUpdateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);

            bool isExsist = await _userManager.Users.AnyAsync(u => u.UserName == dto.UserName && u.Id != dto.Id);
            if (isExsist)
                throw new BusinessValidationException("username already exsist");

            var user = await _userManager.FindByIdAsync(dto.Id) ??
                throw new NotFoundException("user not found.");

            user.UserName = dto.UserName;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => new ValidationFailure(string.Empty, error.Description) { ErrorCode = error.Code }).ToList();
                throw new BusinessValidationException(errors);
            }
        }

        public async Task ChangePasswordAsync(string userId, PasswordChangeDTO dto)
        {
            var validationResult = await _passwordChangeValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);

            var user = await _userManager.FindByIdAsync(userId) ??
                throw new NotFoundException("user not found.");

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => new ValidationFailure(string.Empty, error.Description) { ErrorCode = error.Code }).ToList();
                throw new BusinessValidationException(errors);
            }
        }

        public async Task SendPasswordResetEmailAsync(ForgotPasswordDTO dto, string resetPasswordUrl)
        {
            var validationResult = await _forgotPasswordValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);

            AppUser? user = null;

            if (dto.UserNameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(dto.UserNameOrEmail);
            }

            user ??= await _userManager.FindByNameAsync(dto.UserNameOrEmail)
                 ?? throw new BusinessValidationException("user not found.");

            string rawToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            byte[] tokenBytes = Encoding.UTF8.GetBytes(rawToken);
            string validToken = WebEncoders.Base64UrlEncode(tokenBytes);

            string resetLink = $"{resetPasswordUrl}?userId={user.Id}&token={validToken}";

            await _emailService.SendEmailAsync(
            user.Email!,
            "Reset password",
            $"<a href='{resetLink}'>Click</a> to reset.");
        }

        public async Task ResetPasswordAsync(ResetPasswordDTO dto)
        {
            var validationResult = await _resetPasswordValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);

            var user = await _userManager.FindByIdAsync(dto.Id) ??
                throw new NotFoundException("User not found.");

            byte[] tokenBytes = WebEncoders.Base64UrlDecode(dto.token);
            string decodeToken = Encoding.UTF8.GetString(tokenBytes);

            var result = await _userManager.ResetPasswordAsync(user, decodeToken, dto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => new ValidationFailure(string.Empty, error.Description) { ErrorCode = error.Code }).ToList();
                throw new BusinessValidationException(errors);
            }
        }

        public async Task SendEmailConfirmationAsync(string userId, string clientConfirmUrl)
        {
            var user = await _userManager.FindByIdAsync(userId) ??
                throw new NotFoundException("User not found.");

            if (user.EmailConfirmed)
                throw new BusinessException("This email address is confirmed");

            string rawToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            byte[] tokenBytes = Encoding.UTF8.GetBytes(rawToken);
            string validToken = WebEncoders.Base64UrlEncode(tokenBytes);

            string confirmationLink = $"{clientConfirmUrl}?userId={user.Id}&token={validToken}";

            await _emailService.SendEmailAsync(
            user.Email!,
            "Confirm your email adress!",
            $"<a href='{confirmationLink}'>Click</a> to confirm.");
        }

        public async Task ConfirmEmailAsync(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                throw new BusinessException("Invalid confirm request.");

            var user = await _userManager.FindByIdAsync(userId) ??
                throw new BusinessException("User not found.");

            if (user.EmailConfirmed)
                throw new BusinessException("This email address is confirmed");

            byte[] tokenBytes = WebEncoders.Base64UrlDecode(token);
            string decodeToken = Encoding.UTF8.GetString(tokenBytes);

            var result = await _userManager.ConfirmEmailAsync(user, decodeToken);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => new ValidationFailure(string.Empty, error.Description) { ErrorCode = error.Code }).ToList();
                throw new BusinessValidationException(errors);
            }
        }
    }
}