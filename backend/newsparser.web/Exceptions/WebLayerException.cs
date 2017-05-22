using System;
using System.Net;

namespace NewsParser.Exceptions
{
    public class WebLayerException: Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public WebLayerException() { }

        public WebLayerException(string message): base(message) { }

        public WebLayerException(string message, Exception innerException) 
            : base(message, innerException) { }
        public WebLayerException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}