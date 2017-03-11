using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NewsParser.Helpers.ValidationAttributes
{
    /// <summary>
    /// Attribute validates a string array to contain only digits
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DigitsStringArrayAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var stringValues = value as string[];
            return stringValues.All(s => s.All(char.IsDigit) && !string.IsNullOrEmpty(s)) ? 
                ValidationResult.Success : new ValidationResult("Array must contain only digits");
        }
    }
}
