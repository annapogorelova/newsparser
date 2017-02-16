using System.Linq;
using System.Threading.Tasks;
using NewsParser.BL.Services.NewsSources;
using NewsParser.DAL.Models;
using NewsParser.FeedParser;

namespace newsparser.feedparser
{
    /// <summary>
    /// Class implements IFeedUpdater
    /// </summary>
    public class FeedUpdater: IFeedUpdater
    {
        private readonly INewsSourceBusinessService _newsSourceBusinessService;
        private readonly IFeedParser _feedParser;

        public FeedUpdater(INewsSourceBusinessService newsSourceBusinessService, IFeedParser feedParser)
        {
            _newsSourceBusinessService = newsSourceBusinessService;
            _feedParser = feedParser;
        }

        public void UpdateFeed(int? userId = null, int? sourceId = null)
        {
            if (sourceId == null)
            {
                var newsSources = GetNewsSources(userId).ToList();
                foreach (var newsSource in newsSources)
                {
                    _feedParser.ParseNewsSource(newsSource).Wait();
                }
            }
        }

        public async Task UpdateFeedAsync(int? userId = null, int? sourceId = null)
        {
            if (sourceId == null)
            {
                var newsSources = GetNewsSources(userId).ToList();
                foreach (var newsSource in newsSources)
                {
                    await _feedParser.ParseNewsSource(newsSource);
                }
            }
        }

        private IQueryable<NewsSource> GetNewsSources(int? userId)
        {
            return userId != null
                   ? _newsSourceBusinessService.GetUserNewsSources(userId.Value)
                   : _newsSourceBusinessService.GetNewsSources(true);
        }
    }
}
