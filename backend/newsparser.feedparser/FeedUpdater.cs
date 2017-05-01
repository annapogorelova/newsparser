using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NewsParser.BL.Exceptions;
using NewsParser.BL.Services.News;
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
        private readonly INewsBusinessService _newsBusinessService;
        private readonly IFeedParser _feedParser;
        private readonly ILogger<FeedUpdater> _log;

        public FeedUpdater(INewsSourceBusinessService newsSourceBusinessService, 
            INewsBusinessService newsBusinessService, IFeedParser feedParser, ILogger<FeedUpdater> log)
        {
            _newsSourceBusinessService = newsSourceBusinessService;
            _newsBusinessService = newsBusinessService;
            _feedParser = feedParser;
            _log = log;
        }

        public void UpdateFeed(IEnumerable<NewsSource> newsSources)
        {
            _log.LogInformation("Started updating news sources");

            foreach (var newsSource in newsSources)
            {
                try
                {
                    UpdateSourceAsync(newsSource.Id).Wait();
                }
                catch (Exception e)
                {
                    string errorMessage = $"Failed updating feed: {e.Message}";
                    _log.LogError(errorMessage);
                    throw new FeedUpdatingException(errorMessage, e);
                }
            }

            _log.LogInformation("Finished updating news sources");
        }

        public async Task UpdateFeedAsync(IEnumerable<NewsSource> newsSources)
        {
            _log.LogInformation("Started updating news sources");

            foreach (var newsSource in newsSources)
            {
                try
                {
                    await UpdateSourceAsync(newsSource.Id);
                }
                catch (Exception e)
                {
                    string errorMessage = $"Failed updating feed: {e.Message}";
                    _log.LogError(errorMessage);
                    throw new FeedUpdatingException(errorMessage, e);
                }
            }

            _log.LogInformation("Finished updating news sources");
        }

        public async Task UpdateSourceAsync(int sourceId)
        {
            try
            {
                var newsSource = _newsSourceBusinessService.GetNewsSourceById(sourceId);

                if (newsSource.IsUpdating)
                {
                    _log.LogError($"News source {sourceId} is currently being updated");
                    return;
                }

                SetNewsSourceUpdatingState(newsSource, true);
                var news = await _feedParser.ParseNewsSource(newsSource);
                SaveNewsItems(newsSource.Id, news);
                SetNewsSourceUpdatingState(newsSource, false);
            }
            catch (Exception e)
            {
                string errorMessage = $"Failed updating news source {sourceId}: {e.Message}";

                if(e is EntityNotFoundException)
                {
                    errorMessage = $"Failed updating the feed: source with id {sourceId} does not exist";
                }

                _log.LogError(errorMessage);
                throw new FeedUpdatingException(errorMessage, e);
            }
        }

        public void UpdateSource(int sourceId)
        {
            try
            {
                var newsSource = _newsSourceBusinessService.GetNewsSourceById(sourceId);

                if (newsSource.IsUpdating)
                {
                    _log.LogInformation($"News source {sourceId} is currently being updated");
                    return;
                }

                SetNewsSourceUpdatingState(newsSource, true);
                var news = _feedParser.ParseNewsSource(newsSource).Result;
                SaveNewsItems(newsSource.Id, news);
                SetNewsSourceUpdatingState(newsSource, false);
            }
            catch (Exception e)
            {
                string errorMessage = $"Failed updating news source {sourceId}: {e.Message}";

                if(e is EntityNotFoundException)
                {
                    errorMessage = $"Failed updating the feed: source with id {sourceId} does not exist";
                }

                _log.LogError(errorMessage);
                throw new FeedUpdatingException(errorMessage, e);
            }
        }

        public async Task<NewsSource> AddNewsSource(string rssUrl, int? userId = null)
        {
            if (string.IsNullOrEmpty(rssUrl))
            {
                throw new ArgumentNullException(nameof(rssUrl), "RSS url cannot be null or empty");
            }

            try
            {
                var newsSource = await _feedParser.ParseRssSource(rssUrl);
                var addedNewsSource = _newsSourceBusinessService.AddNewsSource(newsSource);
                UpdateSource(addedNewsSource.Id);

                if (userId.HasValue)
                {
                    _newsSourceBusinessService.AddNewsSourceToUser(addedNewsSource.Id, userId.Value);
                }
                return addedNewsSource;
            }
            catch (Exception e)
            {
                throw new FeedUpdatingException($"Failed to get RSS {rssUrl} source info", e);
            }
        }

        private void SetNewsSourceUpdatingState(NewsSource newsSource, bool isUpdating)
        {
            newsSource.IsUpdating = isUpdating;
            newsSource.DateFeedUpdated = DateTime.UtcNow;
            _newsSourceBusinessService.UpdateNewsSource(newsSource);
        }

        private void SaveNewsItems(int sourceId, List<NewsItemParseModel> newsItems)
        {
            foreach (var newsItem in newsItems)
            {
                if (!_newsBusinessService.NewsItemExists(newsItem.LinkToSource))
                {
                    var addedNewsItem = _newsBusinessService.AddNewsItem(new NewsItem()
                    {
                        SourceId = sourceId,
                        DatePublished = newsItem.DatePublished,
                        Description = newsItem.Description,
                        ImageUrl = newsItem.ImageUrl,
                        LinkToSource = newsItem.LinkToSource,
                        Title = newsItem.Title
                    });
                    
                    _newsBusinessService.AddTagsToNewsItem(addedNewsItem.Id, newsItem.Categories);
                }
            }
        }
    }
}
