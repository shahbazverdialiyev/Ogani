using Ogani.WebApp.Business.DTOs.Auth;
using Ogani.WebApp.DTOs.Auth;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDetailDTO>> GetAllUsersAsync();
        Task<UserDetailDTO> GetUserByIdAsync(int userId);
        Task UpdateUserRolesAsync(UpdateUserRolesDTO dto);
        Task DeleteUserAsync(int userId);
    }
}