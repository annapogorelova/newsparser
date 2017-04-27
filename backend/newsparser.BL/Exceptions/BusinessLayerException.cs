using System;

namespace NewsParser.BL.Exceptions
{
    /// <summary>
    /// Class represents an exception to be thrown by the business layer
    /// </summary>
    public class BusinessLayerException: Exception
    {
        public BusinessLayerException() { }

        public BusinessLayerException(string message): base(message) { }

        public BusinessLayerException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
