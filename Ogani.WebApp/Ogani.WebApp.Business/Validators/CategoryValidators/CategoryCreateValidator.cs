﻿using FluentValidation;
using Ogani.WebApp.DTOs.CategoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Validators.CategoryValidators
{
    public class CategoryCreateValidator : AbstractValidator<CategoryCreateDTO>
    {
        public CategoryCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");
            RuleFor(x => x.Description)
                .Length(10, 200).WithMessage("Description must be between 10 and 200 characters.");
        }
    }
}
