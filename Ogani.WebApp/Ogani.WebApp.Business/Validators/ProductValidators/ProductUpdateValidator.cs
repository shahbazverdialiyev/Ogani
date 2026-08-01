using FluentValidation;
using Ogani.WebApp.DTOs.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Validators.ProductValidators
{
    public class ProductUpdateValidator : ProductBaseValidator<ProductUpdateDTO>
    {
        public ProductUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid product id.");
        }
    }
}
