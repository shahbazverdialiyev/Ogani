using FluentValidation;
using Ogani.WebApp.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Validators.Auth
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDTO>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Invalid reset request.");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Invalid reset request.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Please enter new password.")
                .MinimumLength(3).WithMessage("Password must be at least 3 characters.")
                .MaximumLength(30).WithMessage("Password cannot exceed 30 characters.");
        }
    }
}
