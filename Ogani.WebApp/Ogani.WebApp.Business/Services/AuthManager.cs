using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Ogani.WebApp.Business.Authentication;
using Ogani.WebApp.Business.DTOs.Auth;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Entities.Identity;

namespace Ogani.WebApp.Business.Services
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;

        public AuthManager(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task RegisterAsync(RegisterDTO dto)
        {
            var user = new AppUser
            {
                FullName = dto.FirstName + " " + dto.LastName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(
                user,
                dto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .Select(error => new ValidationFailure(
                        error.Code,
                        error.Description))
                    .ToList();

                throw new BusinessValidationException(errors);
            }

            await _userManager.AddToRoleAsync(user, "Member");
        }

        public async Task<AppUserDTO?> ValidateUserAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user is null)
                return null;

            var isPasswordValid =
                await _userManager.CheckPasswordAsync(
                    user,
                    dto.Password);

            if (!isPasswordValid)
                return null;

            var userRoles = await _userManager.GetRolesAsync(user);

            return new AppUserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                Roles =userRoles.ToList()
            };
        }
    }
}