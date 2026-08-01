using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Exceptions
{
    public class BusinessValidationException:BusinessException
    {
        public IEnumerable<ValidationFailure> Errors { get; }

        public BusinessValidationException(IEnumerable<ValidationFailure> errors)
            : base("Validation failed.")
        {
            Errors = errors;
        }
    }
}
