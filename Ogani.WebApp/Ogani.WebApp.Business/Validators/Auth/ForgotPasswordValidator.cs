using FluentValidation;
using Ogani.WebApp.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Validators.Auth
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDTO>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.UserNameOrEmail)
                .NotEmpty().WithMessage("Username or email is required.")
                .MinimumLength(3)
                .MaximumLength(100)
                .Matches(@"^[a-zA-Z0-9_.\-@]+$").WithMessage("Invalid login credentials.");
        }
    }
}
