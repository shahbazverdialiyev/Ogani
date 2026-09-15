namespace Ogani.WebApp.DTOs.Auth
{
    public class UserProfileDTO
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmNewPassword { get; set; }
    }
}