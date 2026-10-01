using FluentValidation;
using Ogani.WebApp.DTOs.Auth;

namespace Ogani.WebApp.Business.Validators.Auth
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserNameOrEmail)
                .NotEmpty().WithMessage("Username or email is required.")
                .MinimumLength(3)
                .MaximumLength(100)
                .Matches(@"^[a-zA-Z0-9_.\-@]+$").WithMessage("Invalid login credentials.");

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Password is required.")
               .MinimumLength(6)
               .MaximumLength(30).WithMessage("Invalid login credentials.");
        }
    }
}