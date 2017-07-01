using System;

namespace NewsParser.FeedParser.Exceptions
{
    /// <summary>
    /// Class represents the exception to be thrown by FeedParser
    /// </summary>
    public class FeedParsingException : Exception
    {
        public FeedParsingException() { }

        public FeedParsingException(string message): base(message) { }

        public FeedParsingException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
