using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace NewsParser.Helpers.ModelBinders
{
    /// <summary>
    /// Class provides an instance of <see cref="CommaDelimitedArrayModelBinder"/> for  models with string[] type
    /// </summary>
    public class CommaDelimitedArrayModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(string[]))
            {
                return new CommaDelimitedArrayModelBinder(new SimpleTypeModelBinder(context.Metadata.ModelType));
            }

            return null;
        }
    }
}
