using System.Linq;
using FluentScheduler;
using NewsParser.FeedParser;
using Microsoft.Extensions.DependencyInjection;
using newsparser.feedparser;
using newsparser.scheduler;
using NewsParser.BL.Services.NewsSources;

namespace NewsParser.Scheduler
{
    /// <summary>
    /// Class represents a job that's to be scheduled to update RSS feed
    /// </summary>
    public class FeedUpdateJob : IJob
    {
        private readonly IFeedUpdater _feedUpdater;
        private readonly INewsSourceBusinessService _newsSourceBusinessService;
        private readonly object _feedUpadteLock = new object();

        public FeedUpdateJob()
        {
            _feedUpdater = ServiceLocator.Instance.GetService<IFeedUpdater>();
            _newsSourceBusinessService = ServiceLocator.Instance.GetService<INewsSourceBusinessService>();
        }

        public void Execute()
        {
            lock (_feedUpadteLock)
            {
                try
                {
                    var newsSourcs = _newsSourceBusinessService.GetNewsSources(true).ToList();
                    foreach (var newsSource in newsSourcs)
                    {
                        _feedUpdater.UpdateSource(newsSource.Id);
                    }
                }
                catch (FeedParsingException e)
                {
                    throw new JobExecutionException($"Failed to update feed update", e);
                }
            }
        }
    }
}
