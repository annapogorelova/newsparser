using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using NewsParser.DAL.Models;
using NewsParser.Parser;
using NewsParser.BL.NewsSources;
using System.Linq;

namespace NewsParser.Scheduler
{
    /// <summary>
    /// Class represents a job that's to be scheduled to update RSS feed
    /// </summary>
    public class FeedUpdateJob: IJob
    {
        private readonly IFeedParser _feedParser;
        private readonly INewsSourceBusinessService _newsSourceBusinessService;

        private readonly object _feedUpadteLock = new object();

        public FeedUpdateJob()
        {
            _feedParser = ServiceLocator.Instance.GetService<IFeedParser>();
            _newsSourceBusinessService = ServiceLocator.Instance.GetService<INewsSourceBusinessService>();
        }

        public void Execute()
        {
            lock (_feedUpadteLock)
            {
                var newsSources = _newsSourceBusinessService.GetNewsSources(true).ToList();

                foreach (var newsSource in newsSources)
                {
                    ExecuteAsync(newsSource);
                }
            }
        }

        public async void ExecuteAsync(NewsSource newsSource)
        {
            try
            {
                _feedParser.ParseNewsSource(newsSource).Wait();
            }
            catch (FeedParsingException e)
            {
                throw new JobExecutionException($"Failed to execute feed update for {newsSource.Name}", e);
            }
        }
    }
}
