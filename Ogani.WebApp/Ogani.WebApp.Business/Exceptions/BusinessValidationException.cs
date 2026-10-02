using FluentValidation.Results;

namespace Ogani.WebApp.Business.Exceptions
{
    public class BusinessValidationException : BusinessException
    {
        public IEnumerable<ValidationFailure> Errors { get; }

        public BusinessValidationException(IEnumerable<ValidationFailure> errors)
            : base("Validation failed.")
        {
            Errors = errors;
        }

        public BusinessValidationException(ValidationFailure error)
            : this(new List<ValidationFailure>() { error }) { }

        public BusinessValidationException(string errorMesage, string propertyName = "", string errorCode = "VALIDATION EXCEPTION")
            : this(new List<ValidationFailure>(){new ValidationFailure(propertyName, errorMesage)
            {
                ErrorCode = errorCode
            } })
        { }
    }
}