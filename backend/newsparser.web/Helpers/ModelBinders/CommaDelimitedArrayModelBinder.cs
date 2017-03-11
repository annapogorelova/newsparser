using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NewsParser.Helpers.ModelBinders
{
    /// <summary>
    /// Model binder to parse a comma delimited string array
    /// </summary>
    public class CommaDelimitedArrayModelBinder : IModelBinder
    {
        private readonly IModelBinder _fallbackBinder;

        public CommaDelimitedArrayModelBinder(IModelBinder fallbackBinder)
        {
            if (fallbackBinder == null)
                throw new ArgumentNullException(nameof(fallbackBinder));

            _fallbackBinder = fallbackBinder;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);

            if (valueProviderResult.Length > 0)
            {
                var valueAsString = valueProviderResult.FirstValue;

                if (string.IsNullOrEmpty(valueAsString))
                {
                    return _fallbackBinder.BindModelAsync(bindingContext);
                }

                var result = valueAsString.Split(',');
                bindingContext.Result = ModelBindingResult.Success(result);
            }

            return TaskCache.CompletedTask;
        }
    }
}
