using System;

namespace NewsParser.FeedParser.Exceptions
{
    /// <summary>
    /// Class represents the exception to be thrown by FeedUpdater
    /// </summary>
    public class FeedUpdatingException : Exception
    {
        public FeedUpdatingException() { }

        public FeedUpdatingException(string message): base(message) { }

        public FeedUpdatingException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
