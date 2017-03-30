using System;

namespace NewsParser.Identity
{
    /// <summary>
    /// Class represents an exception to be thrown by authentication services
    /// </summary>
    public class AuthException: Exception
    {
        public AuthException() { }

        public AuthException(string message): base(message) { }

        public AuthException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
