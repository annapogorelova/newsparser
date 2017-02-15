using System.Linq;
using NewsParser.BL.Services.NewsSources;
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

        public void UpdateFeed()
        {
            var newsSources = _newsSourceBusinessService.GetNewsSources(true).ToList();
            foreach (var newsSource in newsSources)
            {
                _feedParser.ParseNewsSource(newsSource).Wait();
            }
        }

        public void UpdateSource(int sourceId)
        {
            var newsSource = _newsSourceBusinessService.GetNewsSourceById(sourceId);
            if (newsSource == null)
            {
                throw new FeedParsingException($"Source with id {sourceId} does not exist");
            }

            _feedParser.ParseNewsSource(newsSource).Wait();
        }
    }
}
