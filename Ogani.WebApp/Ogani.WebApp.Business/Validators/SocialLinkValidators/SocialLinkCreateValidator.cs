using FluentValidation;
using Ogani.WebApp.DTOs.SocialLinkDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Validators.SocialLinkValidators
{
    public class SocialLinkCreateValidator:AbstractValidator<SocialLinkCreateDTO>
    {
        public SocialLinkCreateValidator()
        {
            RuleFor(x=>x.Platform)
                .NotEmpty().WithMessage("Platform name is required")
                .MaximumLength(50).WithMessage("Platform name must be maximum 50 characters");

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Platform url is required")
                .MinimumLength(3).WithMessage("Url must be minimum 3 characters");
        }
    }
}
