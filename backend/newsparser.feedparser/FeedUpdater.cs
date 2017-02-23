using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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

        public void UpdateFeed(int? userId = null, int? sourceId = null)
        {
            try
            {
                if (sourceId == null)
                {
                    _log.LogInformation("Started updating news sources");
                    var newsSources = GetNewsSources(userId).ToList();
                    foreach (var newsSource in newsSources)
                    {
                        if (_newsSourceBusinessService.GetNewsSourceById(newsSource.Id).IsUpdating)
                        {
                            continue;
                        }
                        SetNewsSourceUpdatingState(newsSource, true);
                        var news = _feedParser.ParseNewsSource(newsSource).Result;
                        SaveNewsItems(newsSource.Id, news);
                        SetNewsSourceUpdatingState(newsSource, false);
                    }

                    _log.LogInformation("Finished updating news sources");
                }
                else
                {
                    _log.LogInformation($"Started updating news source {sourceId.Value}");
                    UpdateSource(sourceId.Value).Wait();
                    _log.LogInformation($"Finished updating news source {sourceId.Value}");
                }
            }
            catch (FeedParsingException e)
            {
                string errorMessage = $"Failed updating feed: {e.Message}";
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

        public async Task UpdateFeedAsync(int? userId = null, int? sourceId = null)
        {
            try
            {
                if (sourceId == null)
                {
                    _log.LogInformation("Started updating news sources");
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
                        _log.LogInformation("Finished updating news sources");
                    }
                }
                else
                {
                    _log.LogInformation($"Started updating news source {sourceId.Value}");
                    await UpdateSource(sourceId.Value);
                    _log.LogInformation($"Finished updating news source {sourceId.Value}");
                }
            }
            catch (FeedParsingException e)
            {
                string errorMessage = $"Failed updating feed: {e.Message}";
                _log.LogError(errorMessage);
                throw new FeedUpdatingException(errorMessage, e);
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

        private IEnumerable<NewsSource> GetNewsSources(int? userId)
        {
            return userId != null
                   ? _newsSourceBusinessService.GetUserNewsSourcesPage(userId.Value)
                   : _newsSourceBusinessService.GetNewsSources(true);
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
                if (_newsBusinessService.GetNewsItemByLink(newsItem.LinkToSource) == null)
                {
                    var addedNewsItem = _newsBusinessService.AddNewsItem(new NewsItem()
                    {
                        SourceId = sourceId,
                        DateAdded = newsItem.DateAdded,
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
