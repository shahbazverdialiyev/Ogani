using Ogani.WebApp.DTOs.Auth;

namespace Ogani.WebApp.Business.Authentication
{
    public interface IAuthService
    {
        Task<AppUserDTO> RegisterAsync(RegisterDTO dto);

        Task<AppUserDTO> ValidateUserAsync(LoginDTO dto);

        Task<ProfileUpdateDTO> GetUserProfileAsync(string id);

        Task UpdateUserProfileAsync(ProfileUpdateDTO dto);

        Task ChangePasswordAsync(string userId, PasswordChangeDTO dto);

        Task SendPasswordResetEmailAsync(ForgotPasswordDTO dto, string resetPasswordUrl);

        Task ResetPasswordAsync(ResetPasswordDTO dto);

        Task SendEmailConfirmationAsync(string userId, string clientConfirmUrl);

        Task ConfirmEmailAsync(string userId, string token);
    }
}