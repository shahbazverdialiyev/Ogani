using Ogani.WebApp.DTOs.Auth;
using System.ComponentModel.DataAnnotations;

namespace Ogani.WebApp.UI.Models.Account
{
    public record RegisterVM : RegisterDTO
    {
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirmation password is required.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; init; } = null!;
    }
}
