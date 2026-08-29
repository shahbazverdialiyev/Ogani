using FluentValidation;
using Ogani.WebApp.DTOs.UsefulLinkDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Validators.UsefulLinkValidators
{
    public class UsefulLinkUpdateValidator:AbstractValidator<UsefulLinkUpdateDTO>
    {
        public UsefulLinkUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid link id.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name must be maximum 50 characters.");

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Platform url is required.")
                .MinimumLength(3).WithMessage("Url must be minimum 3 characters.")
                .MaximumLength(1000).WithMessage("Url must be maximum 1000 characters.");

            RuleFor(x => x.Section)
                .IsInEnum().WithMessage("Please select section.");
        }
    }
}
