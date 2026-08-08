using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Validators.DiscountValidators
{
    public class DiscountBaseValidator<T>:AbstractValidator<T>
        where T : class
    {
    }
}
