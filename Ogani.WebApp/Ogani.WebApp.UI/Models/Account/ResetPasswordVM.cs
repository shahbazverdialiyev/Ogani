using Ogani.WebApp.DTOs.Auth;
using Ogani.WebApp.UI.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Ogani.WebApp.UI.Models.Account
{
    public record ResetPasswordVM(string id, string token) : ResetPasswordDTO(id, token)
    {
        [RequiredIfNotNull(nameof(NewPassword), ErrorMessage = "Please confirm your new password.")]
        [Compare(nameof(NewPassword), ErrorMessage = "New password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string? ConfirmNewPassword { get; set; }
    }
}