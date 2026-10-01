using FluentValidation;
using Ogani.WebApp.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Validators.Auth
{
    public class PasswordChangeValidator : AbstractValidator<PasswordChangeDTO>
    {
        public PasswordChangeValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required to set a new password.");

            When(x => !string.IsNullOrWhiteSpace(x.CurrentPassword), () =>
            {
                RuleFor(x => x.NewPassword)
                    .NotEmpty().WithMessage("Please enter a new password")
                    .MinimumLength(6).WithMessage("New password must be at least 6 characters.");
            });
        }
    }
}
