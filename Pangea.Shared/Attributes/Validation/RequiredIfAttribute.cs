using Pangea.Shared.Extensions.General;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Pangea.Shared.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        #region Class Members

        public string DependentProperty { get; set; }

        public object TargetValue { get; set; }

        public string ValidationPattern { get; set; }

        public string ValidationMessage { get; set; }

        #endregion
                                                                     
        #region Constructors

        public RequiredIfAttribute
        (
            string dependentProperty,
            object targetValue,
            string validationPattern = "",
            string validationMessage = ""
        )
        {
            DependentProperty = dependentProperty;
            TargetValue = targetValue;

            ValidationPattern = validationPattern;
            ValidationMessage = validationMessage;
        }

        #endregion

        #region Methods

        protected override ValidationResult IsValid(object? value, ValidationContext context)
        {
            object instance = context.ObjectInstance;

            if (instance.GetType().GetProperties().Any(prop => prop.Name == DependentProperty))
            {
                object dependentPropertyValue = instance.GetType().GetProperty(DependentProperty)?.GetValue(instance, null)!;

                if (TargetValue.Equals(dependentPropertyValue))
                {
                    if (value == null)
                    {
                        return new ValidationResult
                        (
                            ErrorMessage.HasValue()
                            ? ErrorMessage
                            : $"{context.DisplayName} is required."
                        );
                    }

                    if (ValidationPattern.HasValue())
                    {
                        Regex rx = new(ValidationPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                        return 
                        (
                            rx.IsMatch(value.ToString()!)
                            ? ValidationResult.Success
                            : new ValidationResult
                            (
                                ValidationMessage.HasValue()
                                ? ValidationMessage
                                : $"{context.DisplayName} value format is incorrect."
                            )
                        )!;
                    }

                    return ValidationResult.Success!;
                }

                return ValidationResult.Success!;
            }

            return new ValidationResult("Dependent property not found!");
        }

        #endregion
    }
}
