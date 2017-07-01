
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NewsParser.Helpers.ActionFilters.ModelValidation
{
    public class ValidationResultModel
    {
        public string Message { get; } 

        public List<ValidationError> ValidationErrors { get; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            Message = "Validation failed";
            ValidationErrors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors
                    .Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
        }
    }
}