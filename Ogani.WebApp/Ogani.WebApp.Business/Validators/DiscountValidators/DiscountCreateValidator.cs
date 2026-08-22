using FluentValidation;
using Ogani.WebApp.DTOs.DiscountDTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Validators.DiscountValidators
{
    using FluentValidation;

    public class DiscountCreateDtoValidator : AbstractValidator<DiscountCreateDTO>
    {
        public DiscountCreateDtoValidator()
        {
            RuleFor(d => d.Code)
                .NotEmpty().WithMessage("Discount code is required.")
                .MaximumLength(50).WithMessage("Discount code must not exceed 50 characters.")
                .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Discount code must contain ASCII characters only."); // IsUnicode(false)

            RuleFor(d => d.DiscountPercentage)
                .InclusiveBetween(0.01m, 100.00m).WithMessage("Discount percentage must be between 0.01 and 100.00.")
                .ScalePrecision(2, 5).WithMessage("Discount percentage cannot exceed 5 digits in total, with 2 decimal places.");

            RuleFor(d => d.StartDate)
                .NotEmpty().WithMessage("Start date is required.")
                .GreaterThanOrEqualTo(_ => DateTime.UtcNow.Date).WithMessage("Start date cannot be in the past.");

            RuleFor(d => d.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThan(d => d.StartDate).WithMessage("End date must be later than the start date.");
        }
    }
}
