using FluentValidation;
using Ogani.WebApp.DTOs.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Validators.ProductValidators
{
    public abstract class ProductBaseValidator<T> : AbstractValidator<T>
        where T : IProductRequest
    {
        private static readonly string[] AllowedExtensionsForImage =
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };
        private const int MaxImageSizeInMB = 2;
        private const long MaxImageSize = MaxImageSizeInMB * 1024 * 1024;

        protected ProductBaseValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(2).WithMessage("Name must be minimum 2 characters.")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0.");

            RuleFor(x => x.Weight)
                .GreaterThanOrEqualTo(0).WithMessage("Weight must be greater than or equal to 0.");

            When(x => x.Image is not null, () =>
            {
                RuleFor(x => x.Image!)
                    .Must(file => AllowedExtensionsForImage.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
                    .WithMessage($"Only the following file types are allowed: {string.Join(", ", AllowedExtensionsForImage)}.")

                    .Must(file => file.Length <= MaxImageSize)
                    .WithMessage($"Image size must be less than {MaxImageSizeInMB} MB.");
            });

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).When(x => x.CategoryId.HasValue).WithMessage("Please select a valid category.");

            RuleForEach(x => x.DiscountIds)
                .GreaterThan(0)
                .WithMessage("Each selected discount must have a valid ID.");

            RuleFor(x => x.DiscountIds)
                .Must(ids => ids.Count == new HashSet<int>(ids).Count)
                .WithMessage("Duplicate discounts are not allowed.");
        }
    }
}
