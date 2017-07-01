using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NewsParser.API.V1.Models;
using NewsParser.BL.Exceptions;
using NewsParser.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace NewsParser.Middleware
{
    /// <summary>
    /// Http error handler
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _log;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> log)
        {
            _next = next;
            _log = log;
        }

        public async Task Invoke(HttpContext context)
        {
            using (var responseStream = new MemoryStream())
            {
                var fullResponse = context.Response.Body;
                context.Response.Body = responseStream;
                try
                {
                    await _next.Invoke(context);
                }
                catch (Exception ex)
                {
                    HandleExceptionAsync(context, ex);
                }
                finally
                {
                    responseStream.Seek(0, SeekOrigin.Begin);
                    await responseStream.CopyToAsync(fullResponse);
                }
            }
        }

        private void HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _log.LogError(ex.Message);
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            
            if(ex is WebLayerException)
            {
                statusCode = ((WebLayerException)ex).StatusCode;
            }
            
            if(ex is EntityNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
            }


            var result = JsonConvert.SerializeObject(new ErrorModel { Message = ex.Message }, 
                new JsonSerializerSettings 
                { 
                    ContractResolver = new CamelCasePropertyNamesContractResolver() 
                });
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = (int)statusCode;

            var responseStream = context.Response.Body;
            responseStream.Seek(0, SeekOrigin.Begin);
            
            using (var writer = new StreamWriter(responseStream, Encoding.UTF8, 4096, true))
            {
                writer.Write(result);
                writer.Flush();
                responseStream.Flush();
            }
        }
    }
}