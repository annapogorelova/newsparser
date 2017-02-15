using FluentScheduler;

namespace NewsParser.Scheduler
{
    /// <summary>
    /// Class starts the scheduled jobs
    /// </summary>
    public class JobRegistry: Registry
    {
        public JobRegistry(int interval)
        {
            Schedule<FeedUpdateJob>().NonReentrant().ToRunNow().AndEvery(interval).Minutes();
        }
    }
}
