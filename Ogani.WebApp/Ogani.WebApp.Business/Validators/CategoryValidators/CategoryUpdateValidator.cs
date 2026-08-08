using FluentValidation;
using Ogani.WebApp.DTOs.CategoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Validators.CategoryValidators
{
    public class CategoryUpdateValidator : AbstractValidator<CategoryUpdateDTO>
    {
        private static readonly string[] AllowedExtensionsForImage =
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };
        private const int MaxImageSizeInMB = 2;
        private const long MaxImageSize = MaxImageSizeInMB * 1024 * 1024;

        public CategoryUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid category id.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(2).WithMessage("Name must be minimum 2 characters.")
                .MaximumLength(50).WithMessage("Name cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            When(x => x.Image is not null, () =>
            {
                RuleFor(x => x.Image!)
                    .Must(file => AllowedExtensionsForImage.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
                    .WithMessage($"Only the following file types are allowed: {string.Join(", ", AllowedExtensionsForImage)}.")

                    .Must(file => file.Length <= MaxImageSize)
                    .WithMessage($"Image size must be less than {MaxImageSizeInMB} MB.");
            });
        }
    }
}
