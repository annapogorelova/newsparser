using System;
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
            try
            {
                if (sourceId == null)
                {
                    var newsSources = GetNewsSources(userId).ToList();
                    foreach (var newsSource in newsSources)
                    {
                        if (_newsSourceBusinessService.GetNewsSourceById(newsSource.Id).IsUpdating)
                        {
                            continue;
                        }
                        SetNewsSourceUpdatingState(newsSource, true);
                        _feedParser.ParseNewsSource(newsSource).Wait();
                        SetNewsSourceUpdatingState(newsSource, false);
                    }
                }
                else
                {
                    UpdateSource(sourceId.Value).Wait();
                }
            }
            catch (FeedParsingException e)
            {
                throw new FeedUpdatingException("Failed updating feed", e);
            }
        }

        public async Task UpdateFeedAsync(int? userId = null, int? sourceId = null)
        {
            try
            {
                if (sourceId == null)
                {
                    var newsSources = GetNewsSources(userId).ToList();
                    foreach (var newsSource in newsSources)
                    {
                        if (_newsSourceBusinessService.GetNewsSourceById(newsSource.Id).IsUpdating)
                        {
                            continue;
                        }
                        SetNewsSourceUpdatingState(newsSource, true);
                        await _feedParser.ParseNewsSource(newsSource);
                        SetNewsSourceUpdatingState(newsSource, false);
                    }
                }
                else
                {
                    await UpdateSource(sourceId.Value);
                }
            }
            catch (FeedParsingException e)
            {
                throw new FeedUpdatingException("Failed updating feed", e);
            }
        }

        private async Task UpdateSource(int sourceId, int? userId = null)
        {
            var newsSource = _newsSourceBusinessService.GetNewsSourceById(sourceId);
            if (userId != null && newsSource.Users.All(u => u.Id != userId.Value))
            {
                throw new FeedUpdatingException($"Failed updating the feed: user with id {userId.Value} " +
                                                $"is not subscribed to source with id {sourceId}");
            }
            if (newsSource == null)
            {
                throw new FeedUpdatingException($"Failed updating the feed: source with id {sourceId} does not exist");
            }

            SetNewsSourceUpdatingState(newsSource, true);
            await _feedParser.ParseNewsSource(newsSource);
            SetNewsSourceUpdatingState(newsSource, false);
        }

        private IQueryable<NewsSource> GetNewsSources(int? userId)
        {
            return userId != null
                   ? _newsSourceBusinessService.GetUserNewsSources(userId.Value)
                   : _newsSourceBusinessService.GetNewsSources(true);
        }

        private void SetNewsSourceUpdatingState(NewsSource newsSource, bool isUpdating)
        {
            newsSource.IsUpdating = isUpdating;
            newsSource.DateFeedUpdated = DateTime.UtcNow;
            _newsSourceBusinessService.UpdateNewsSource(newsSource);
        }
    }
}
