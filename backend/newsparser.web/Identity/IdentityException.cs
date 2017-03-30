using System;

namespace NewsParser.Identity
{
    /// <summary>
    /// Class represents an exception to be thrown by identity services
    /// </summary>
    public class IdentityException: Exception
    {
        public IdentityException() { }

        public IdentityException(string message): base(message) { }

        public IdentityException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
