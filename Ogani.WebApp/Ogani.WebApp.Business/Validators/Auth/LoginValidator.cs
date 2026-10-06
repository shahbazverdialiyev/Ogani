using FluentValidation;
using Ogani.WebApp.DTOs.Auth;

namespace Ogani.WebApp.Business.Validators.Auth
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            #region UserNameOrEmail Validation

            RuleFor(x => x.UserNameOrEmail)
                .NotEmpty()
                .WithMessage("Username or email is required.");

            RuleFor(x => x.UserNameOrEmail)
                .Length(3, 100)
                .WithMessage("Invalid login credentials.")
                .Matches(@"^[a-zA-Z0-9_.\-@]+$")
                .WithMessage("Invalid login credentials.")
                .OverridePropertyName(string.Empty);

            #endregion

            #region Password Validation

            RuleFor(x => x.Password)
               .NotEmpty()
               .WithMessage("Password is required.");

            RuleFor(x => x.Password)
               .Length(6, 30)
                .WithMessage("Invalid login credentials.")
               .OverridePropertyName(string.Empty);

            #endregion
        }
    }
}