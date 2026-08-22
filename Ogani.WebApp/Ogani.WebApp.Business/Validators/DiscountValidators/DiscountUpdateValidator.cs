using FluentValidation;
using Ogani.WebApp.Business.Validators.ProductValidators;
using Ogani.WebApp.DTOs.DiscountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Validators.DiscountValidators
{
    public class DiscountUpdateValidator : AbstractValidator<DiscountUpdateDTO>
    {
        public DiscountUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid discount id.");

            RuleFor(d => d.Code)
                .NotEmpty().WithMessage("Discount code is required.")
                .MaximumLength(50).WithMessage("Discount code must not exceed 50 characters.")
                .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Discount code must contain ASCII characters only."); // IsUnicode(false)

            RuleFor(d => d.DiscountPercentage)
                .NotNull().WithMessage("Discount percentage is required.")
                .InclusiveBetween(0.01m, 100.00m).WithMessage("Discount percentage must be between 0.01 and 100.00.")
                .ScalePrecision(2, 5).WithMessage("Discount percentage cannot exceed 5 digits in total, with 2 decimal places.");
        }
    }
}
