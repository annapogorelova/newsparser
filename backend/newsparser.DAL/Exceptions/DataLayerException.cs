using System;

namespace NewsParser.DAL.Exceptions
{
    public class DataLayerException : Exception
    {
        public DataLayerException() { }

        public DataLayerException(string message): base(message) { }

        public DataLayerException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}