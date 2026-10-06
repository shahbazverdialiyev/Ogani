using System.ComponentModel.DataAnnotations;

namespace Ogani.WebApp.UI.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredIfNotNullAttribute : ValidationAttribute
    {
        private readonly string[] _propertyNames;

        public RequiredIfNotNullAttribute(params string[] propertyNames)
        {
            _propertyNames = propertyNames;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            bool allPropertiesAreNotNull = true;

            foreach (var propertyName in _propertyNames)
            {
                var property = validationContext.ObjectType.GetProperty(propertyName);

                if (property == null)
                {
                    return new ValidationResult($"Unknown property: {propertyName}");
                }

                var propertyValue = property.GetValue(validationContext.ObjectInstance, null);

                if (propertyValue == null || (propertyValue is string str && string.IsNullOrWhiteSpace(str)))
                {
                    allPropertiesAreNotNull = false;
                    break;
                }
            }

            if (allPropertiesAreNotNull)
            {
                string? currentValue = value as string;

                if (value == null || (currentValue != null && string.IsNullOrWhiteSpace(currentValue)))
                {
                    string errorMessage = ErrorMessage ?? $"{validationContext.DisplayName} is required.";
                    return new ValidationResult(errorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}