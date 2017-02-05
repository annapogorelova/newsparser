using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using NewsParser.DAL.Models;
using NewsParser.DAL.NewsSources;
using NewsParser.Parser;
using System.Linq;

namespace NewsParser.Scheduler
{
    /// <summary>
    /// Class represents a job that's to be scheduled to update RSS feed
    /// </summary>
    public class FeedUpdateJob: IJob
    {
        private readonly IFeedParser _feedParser;
        private readonly INewsSourceRepository _newsSourceRepository;

        private readonly object _feedUpadteLock = new object();

        public FeedUpdateJob()
        {
            _feedParser = ServiceLocator.Instance.GetService<IFeedParser>();
            _newsSourceRepository = ServiceLocator.Instance.GetService<INewsSourceRepository>();
        }

        public void Execute()
        {
            lock (_feedUpadteLock)
            {
                var newsSources = _newsSourceRepository.GetNewsSources().ToList();

                foreach (var newsSource in newsSources)
                {
                    ExecuteAsync(newsSource);
                }
            }
        }

        public async void ExecuteAsync(NewsSource newsSource)
        {
            _feedParser.Parse(newsSource).Wait();
        }
    }
}
