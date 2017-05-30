using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NewsParser.BL.Exceptions;
using NewsParser.BL.Services.News;
using NewsParser.BL.Services.NewsSources;
using NewsParser.DAL.Models;
using NewsParser.FeedParser.Models;
using NewsParser.FeedParser.Exceptions;
using NewsParser.FeedParser.Helpers;
using System.Xml.Linq;
using System.Net.Http;
using NewsParser.FeedParser.Services.FeedSourceParser;
using NewsParser.FeedParser.Services;

namespace newsparser.FeedParser.Services
{
    /// <summary>
    /// Class implements IFeedUpdater
    /// </summary>
    public class FeedUpdater: IFeedUpdater
    {
        private readonly INewsSourceBusinessService _newsSourceBusinessService;
        private readonly INewsBusinessService _newsBusinessService;
        private readonly ILogger<FeedUpdater> _log;

        private readonly Dictionary<FeedFormat, IFeedParser> _feedParsers = 
            new Dictionary<FeedFormat, IFeedParser>
        {
            { FeedFormat.RSS, new RssFeedParser() },
            { FeedFormat.Atom, new AtomFeedParser() }
        };

        private readonly IFeedConnector _feedConnector;

        public FeedUpdater(
            INewsSourceBusinessService newsSourceBusinessService, 
            INewsBusinessService newsBusinessService, 
            ILogger<FeedUpdater> log,
            IFeedConnector feedConnector)
        {
            _newsSourceBusinessService = newsSourceBusinessService;
            _newsBusinessService = newsBusinessService;
            _log = log;
            _feedConnector = feedConnector;
        }

        public void UpdateFeed(IEnumerable<NewsSource> newsSources)
        {
            _log.LogInformation("Started updating news sources");

            foreach (var newsSource in newsSources)
            {
                try
                {
                    UpdateFeedSourceAsync(newsSource.Id).Wait();
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
                    await UpdateFeedSourceAsync(newsSource.Id);
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

        public async Task UpdateFeedSourceAsync(int sourceId)
        {
            var newsSource = _newsSourceBusinessService.GetNewsSourceById(sourceId);
            if (newsSource.IsUpdating)
            {
                _log.LogError($"News source {sourceId} is currently being updated");
                return;
            }

            try
            {
                SetNewsSourceUpdatingState(newsSource, true);
                var news = await _feedConnector.GetFeed(newsSource.FeedUrl, GetNewsSourceFeedFormat(newsSource));
                SaveFeedItems(newsSource.Id, news);
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
                SetNewsSourceUpdatingState(newsSource, false);
                throw new FeedUpdatingException(errorMessage, e);
            }
        }

        public void UpdateFeedSource(int sourceId)
        {
            var newsSource = _newsSourceBusinessService.GetNewsSourceById(sourceId);
            if (newsSource.IsUpdating)
            {
                _log.LogInformation($"News source {sourceId} is currently being updated");
                return;
            }

            try
            {
                SetNewsSourceUpdatingState(newsSource, true);
                var news = _feedConnector.GetFeed(newsSource.FeedUrl, newsSource.FeedFormat).Result;
                SaveFeedItems(newsSource.Id, news);
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
                SetNewsSourceUpdatingState(newsSource, false);
                throw new FeedUpdatingException(errorMessage, e);
            }
        }

        public async Task<NewsSource> AddFeedSource(string feedUrl, bool isPrivate, int userId)
        {
            if (string.IsNullOrEmpty(feedUrl))
            {
                throw new ArgumentNullException(nameof(feedUrl), "RSS url cannot be null or empty");
            }

            try
            {
                var feedSourceModel = await _feedConnector.GetFeedSource(feedUrl);
                var newsSource = new NewsSource
                {
                    Name = feedSourceModel.Name?
                            .RemoveTabulation(" ")
                            .RemoveHtmlTags()
                            .RemoveNonAlphanumericCharacters()
                            .CropString(100) ?? "Untitled",
                    Description = feedSourceModel.Description?
                            .RemoveTabulation(" ")
                            .RemoveHtmlTags()
                            .RemoveNonAlphanumericCharacters()
                            .CropString(255),
                    ImageUrl = feedSourceModel.ImageUrl,
                    FeedUrl = feedSourceModel.FeedUrl,
                    WebsiteUrl = feedSourceModel.WebsiteUrl,
                    FeedFormat = feedSourceModel.FeedFormat,
                    UpdateIntervalMinutes = feedSourceModel.UpdateIntervalMinutes
                };

                var addedFeedSource = _newsSourceBusinessService.AddNewsSource(newsSource);
                UpdateFeedSource(addedFeedSource.Id);
                _newsSourceBusinessService.SubscribeUser(addedFeedSource.Id, userId, isPrivate);
                
                return addedFeedSource;
            }
            catch (Exception e)
            {
                throw new FeedUpdatingException($"Failed to get RSS {feedUrl} source info", e);
            }
        }

        private void SetNewsSourceUpdatingState(NewsSource newsSource, bool isUpdating)
        {
            newsSource.IsUpdating = isUpdating;
            newsSource.DateFeedUpdated = DateTime.UtcNow;
            _newsSourceBusinessService.UpdateNewsSource(newsSource);
        }

        private void SaveFeedItems(int sourceId, List<FeedItem> feedItems)
        {
            foreach (var feedItem in feedItems)
            {
                NewsItem existingItem;
                if (!_newsBusinessService.NewsItemExists(feedItem.Id))
                {
                    try
                    {
                        existingItem = _newsBusinessService.AddNewsItem(new NewsItem()
                        {
                            DatePublished = feedItem.DatePublished,
                            Description = feedItem.Description?
                                .RemoveTabulation(" ")
                                .RemoveHtmlTags()
                                .RemoveNonAlphanumericCharacters()
                                .CropString(500),
                            ImageUrl = feedItem.ImageUrl,
                            LinkToSource = feedItem.Link,
                            Title = feedItem.Title?
                                .RemoveTabulation(" ")
                                .RemoveHtmlTags()
                                .RemoveNonAlphanumericCharacters()
                                .CropString(255) ?? "Untitled",
                            Guid = feedItem.Id
                        });
                    }
                    catch(Exception e)
                    {
                        _log.LogError($"Failed to add a news item {feedItem.Id}", e);
                        continue;
                    }
                }
                else 
                {
                    existingItem = _newsBusinessService.GetNewsItemByGuid(feedItem.Id);
                }

                _newsBusinessService.UpdateNewsItem(existingItem.Id, sourceId, feedItem.Categories);
            }
        }

        private FeedFormat GetNewsSourceFeedFormat(NewsSource source)
        {
            return (FeedFormat)((int)source.FeedFormat);
        }
    }
}
