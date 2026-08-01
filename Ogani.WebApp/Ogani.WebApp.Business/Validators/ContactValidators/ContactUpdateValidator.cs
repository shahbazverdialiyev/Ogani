using FluentValidation;
using Ogani.WebApp.DTOs.ContactDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Validators.ContactValidators
{
    public class ContactUpdateValidator : AbstractValidator<ContactUpdateDTO>
    {
        public ContactUpdateValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(50).WithMessage("Title must be maximum 50 characters");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required")
                .MaximumLength(200).WithMessage("Content must be maximum 200 characters");
        }
    }
}
