using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using NewsParser.Helpers.ModelBinders;

namespace NewsParser.Helpers.Extensions
{
    public static class MvcOptionsExtensions
    {
        /// <summary>
        /// Adds a <see cref="CommaDelimitedArrayModelBinder"/> to the list of model binders
        /// </summary>
        /// <param name="opts"></param>
        public static void UseCommaDelimitedArrayModelBinding(this MvcOptions opts)
        {
            var binderToFind = opts.ModelBinderProviders.FirstOrDefault(x => x.GetType() == typeof(SimpleTypeModelBinderProvider));

            if (binderToFind == null) return;

            var index = opts.ModelBinderProviders.IndexOf(binderToFind);
            opts.ModelBinderProviders.Insert(index, new CommaDelimitedArrayModelBinderProvider());
        }
    }
}
