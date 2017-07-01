using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using NewsParser.Helpers.Extensions;

namespace NewsParser.Cache
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAttribute : ResultFilterAttribute, IActionFilter
    {
        protected ICacheService _cacheService { set; get; }
        public int Duration { set; get; }
        public bool DeferByUser { get; set; } = false;

        public CacheAttribute()
        {
            _cacheService = ServiceLocator.Instance.GetService<ICacheService>();
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            SetResultCache(context.HttpContext);
            
            base.OnResultExecuted(context);
        }

        private void SetResultCache(HttpContext context)
        {
            var cacheKey = GetCacheKey(context);
            var httpResponse = context.Response;
            var responseStream = httpResponse.Body;

            responseStream.Seek(0, SeekOrigin.Begin);

            using (var streamReader = new StreamReader(responseStream, Encoding.UTF8, true, 512, true))
            {
                var toCache = streamReader.ReadToEnd();
                var contentType = httpResponse.ContentType;
                var statusCode = httpResponse.StatusCode.ToString();
                Task.Factory.StartNew(() =>
                {
                    _cacheService.Store(cacheKey + "_contentType", contentType, Duration);
                    _cacheService.Store(cacheKey + "_statusCode", statusCode, Duration);
                    _cacheService.Store(cacheKey, toCache, Duration);
                });
            }
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ContentResult)
            {
                context.Cancel = true;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var queryString = context.HttpContext.Request.Query;
            StringValues refreshResult;
            bool parseSucceeded = queryString.TryGetValue("refresh", out refreshResult);
            bool refresh = parseSucceeded ? bool.Parse(refreshResult) : false;

            if(refresh)
            {
                return;
            }
            
            ReturnCachedResult(context);
        }

        private void ReturnCachedResult(ActionExecutingContext context)
        {
            var requestUrl = context.HttpContext.Request.GetEncodedUrl();
            var cacheKey = GetCacheKey(context.HttpContext);
            var cachedResult = _cacheService.Get<string>(cacheKey);
            var contentType = _cacheService.Get<string>(cacheKey + "_contentType");
            var statusCode = _cacheService.Get<string>(cacheKey + "_statusCode");
            if (!string.IsNullOrEmpty(cachedResult) && !string.IsNullOrEmpty(contentType) &&
                !string.IsNullOrEmpty(statusCode))
            {
                var httpResponse = context.HttpContext.Response;
                httpResponse.ContentType = contentType;
                httpResponse.StatusCode = Convert.ToInt32(statusCode);

                var responseStream = httpResponse.Body;
                responseStream.Seek(0, SeekOrigin.Begin);
                if (responseStream.Length <= cachedResult.Length)
                {
                    responseStream.SetLength((long)cachedResult.Length << 1);
                }
                
                using (var writer = new StreamWriter(responseStream, Encoding.UTF8, 4096, true))
                {
                    writer.Write(cachedResult);
                    writer.Flush();
                    responseStream.Flush();
                    context.Result = new ContentResult { Content = cachedResult };
                }
            }
        }

        private string GetCacheKey(HttpContext context)
        {
            string requestUrl = GetNormalizedUrl(context.Request.GetEncodedUrl());

            if(DeferByUser)
            {
                if(context.User == null)
                {
                    throw new CachingException("Failed to retrieve the authorized user for caching");
                }

                requestUrl = $"{requestUrl}__{context.User.Identity.Name}";
            }

            return requestUrl.ToMD5HashString();
        }

        private string GetNormalizedUrl(string url)
        {
            var uri = new Uri(url);
            var baseUri = uri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.UriEscaped);
            var query = QueryHelpers.ParseQuery(uri.Query);
            var items = query.SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();

            items.RemoveAll(x => x.Key == "refresh");
            items = items.OrderBy(item => item.Key).ToList();

            var qb = new QueryBuilder(items);
            var fullUri = baseUri + qb.ToQueryString();

            return fullUri;
        }
    }
}