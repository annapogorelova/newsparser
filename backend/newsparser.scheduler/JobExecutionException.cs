using System;

namespace NewsParser.Scheduler
{
    /// <summary>
    /// Class represents the exception to be thrown by Scheduler
    /// </summary>
    public class JobExecutionException: Exception
    {
        public JobExecutionException() { }

        public JobExecutionException(string message): base(message) { }

        public JobExecutionException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
