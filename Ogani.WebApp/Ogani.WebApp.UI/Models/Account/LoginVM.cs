using Ogani.WebApp.DTOs.Auth;
using System.ComponentModel.DataAnnotations;

namespace Ogani.WebApp.UI.Models.Account
{
    public record LoginVM : LoginDTO
    {
        [Display(Name = "Remember me")]
        public bool RememberMe { get; init; }
    }
}
