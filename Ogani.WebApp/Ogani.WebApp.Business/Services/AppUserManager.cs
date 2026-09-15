using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ogani.WebApp.Business.DTOs.Auth;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.Auth;
using Ogani.WebApp.Entities.Identity;

namespace Ogani.WebApp.Business.Services
{
    public class AppUserManager : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public AppUserManager(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserDetailDTO>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<UserDetailDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDetailDTO
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email!,
                    EmailConfirmed = user.EmailConfirmed,
                    Roles = roles.ToList()
                });
            }

            return userDtos;
        }

        public async Task<UserDetailDTO> GetUserByIdAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                throw new KeyNotFoundException("User not found.");

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDetailDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                EmailConfirmed = user.EmailConfirmed,
                Roles = roles.ToList()
            };
        }

        public async Task UpdateUserRolesAsync(UpdateUserRolesDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            if (user is null)
                throw new KeyNotFoundException("User not found.");

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                throw new InvalidOperationException("Failed to remove existing roles.");

            var addResult = await _userManager.AddToRolesAsync(user, dto.Roles);
            if (!addResult.Succeeded)
                throw new InvalidOperationException("Failed to assign new roles.");
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                throw new KeyNotFoundException("User not found.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to delete user.");
        }
    }
}