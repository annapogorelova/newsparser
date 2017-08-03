using System;

namespace NewsParser.FeedParser.Exceptions
{
    /// <summary>
    /// Class represents the exception to be thrown by FeedUpdater
    /// </summary>
    public class FatalFeedUpdatingException : Exception
    {
        public FatalFeedUpdatingException() { }

        public FatalFeedUpdatingException(string message): base(message) { }

        public FatalFeedUpdatingException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
