using System;

namespace NewsParser.BL.Exceptions
{
    /// <summary>
    /// Class represents an exception to be thrown by the business layer when entity was not found
    /// </summary>
    public class EntityNotFoundException: Exception
    {
        public EntityNotFoundException() { }

        public EntityNotFoundException(string message): base(message) { }

        public EntityNotFoundException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
