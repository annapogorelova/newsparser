using System;

namespace NewsParser.FeedParser.Exceptions
{
    /// <summary>
    /// Class represents the exception to be thrown by FeedParser
    /// </summary>
    public class UnsupportedFeedFormatException : Exception
    {
        public UnsupportedFeedFormatException() { }

        public UnsupportedFeedFormatException(string message): base(message) { }

        public UnsupportedFeedFormatException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
