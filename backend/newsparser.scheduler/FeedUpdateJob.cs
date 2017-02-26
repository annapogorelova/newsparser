using System.Threading.Tasks;
using FluentScheduler;
using NewsParser.FeedParser;
using Microsoft.Extensions.DependencyInjection;
using newsparser.feedparser;
using newsparser.scheduler;

namespace NewsParser.Scheduler
{
    /// <summary>
    /// Class represents a job that's to be scheduled to update RSS feed
    /// </summary>
    public class FeedUpdateJob : IJob
    {
        private readonly IFeedUpdater _feedUpdater;
        private readonly object _feedUpadteLock = new object();

        public FeedUpdateJob()
        {
            _feedUpdater = ServiceLocator.Instance.GetService<IFeedUpdater>();
        }

        public void Execute()
        {
            lock (_feedUpadteLock)
            {
                try
                {
                    _feedUpdater.UpdateFeedAsync().Wait();
                }
                catch (FeedParsingException e)
                {
                    throw new JobExecutionException($"Failed to update feed update", e);
                }
            }
        }
    }
}
