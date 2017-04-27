using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace NewsParser.Helpers.ValidationAttributes
{
    /// <summary>
    /// Attribute validates a string array to contain only digits
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotEqualAttribute: ValidationAttribute
    {
        public string PropertyName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo targetPropertyInfo = validationContext.ObjectType.GetProperty(PropertyName);
            var targetPropertyStringValue = targetPropertyInfo.GetValue(validationContext.ObjectInstance, null).ToString();

            if (value.ToString() == targetPropertyStringValue)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }
    }
}
