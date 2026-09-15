using Ogani.WebApp.Business.DTOs.Auth;

namespace Ogani.WebApp.Business.Authentication
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDTO dto);

        Task<AppUserDTO?> ValidateUserAsync(LoginDTO dto);
    }
}