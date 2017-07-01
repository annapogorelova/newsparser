using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NewsParser.Helpers.ActionFilters.ModelValidation
{
    /// <summary>
    /// Attribute handles model validation
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateModelAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }
    }
}
