using FluentValidation;
using Ogani.WebApp.Business.Extensions;
using Ogani.WebApp.DTOs.HeroDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Validators.HeroValidators
{
    public class HeroCreateDTOValidator : AbstractValidator<HeroCreateDTO>
    {
        public HeroCreateDTOValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Subtitle)
                .NotEmpty().WithMessage("Subtitle is required.")
                .MaximumLength(200).WithMessage("Subtitle cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.ButtonText)
                .NotEmpty().WithMessage("Button text is required.")
                .MaximumLength(50).WithMessage("Button text cannot exceed 50 characters.");

            RuleFor(x => x.ButtonUrl)
                .NotEmpty().WithMessage("Button URL is required.")
                .MaximumLength(200).WithMessage("Button URL cannot exceed 200 characters.");

            RuleFor(x => x.Image)
                .NotNull().WithMessage("Image is required.")
                .ValidateImage();
        }
    }
}
