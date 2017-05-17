using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NewsParser.Cache
{
    public class CacheMiddleware
    {
        protected RequestDelegate NextMiddleware;

        public CacheMiddleware(RequestDelegate nextMiddleware)
        {
            NextMiddleware = nextMiddleware;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (var responseStream = new MemoryStream())
            {
                var fullResponse = httpContext.Response.Body;
                httpContext.Response.Body = responseStream;
                await NextMiddleware.Invoke(httpContext);
                responseStream.Seek(0, SeekOrigin.Begin);
                await responseStream.CopyToAsync(fullResponse);
            }
        }
    }
}