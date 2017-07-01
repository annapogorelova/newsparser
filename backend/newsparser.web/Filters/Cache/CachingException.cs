using System;

namespace NewsParser.Cache
{
    /// <summary>
    /// Exception to be thrown when request/response caching failed
    /// </summary>
    public class CachingException: Exception
    {
        public CachingException() { }

        public CachingException(string message): base(message) { }

        public CachingException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}