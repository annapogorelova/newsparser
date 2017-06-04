using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NewsParser.BL.Exceptions;
using NewsParser.BL.Services.Feed;
using NewsParser.BL.Services.Channels;
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
        private readonly IChannelDataService _channelDataService;
        private readonly IFeedDataService _feedDataService;
        private readonly ILogger<FeedUpdater> _log;

        private readonly Dictionary<FeedFormat, IFeedParser> _feedParsers = 
            new Dictionary<FeedFormat, IFeedParser>
        {
            { FeedFormat.RSS, new RssFeedParser() },
            { FeedFormat.Atom, new AtomFeedParser() }
        };

        private readonly IFeedConnector _feedConnector;

        public FeedUpdater(
            IChannelDataService channelDataService, 
            IFeedDataService feedDataService, 
            ILogger<FeedUpdater> log,
            IFeedConnector feedConnector)
        {
            _channelDataService = channelDataService;
            _feedDataService = feedDataService;
            _log = log;
            _feedConnector = feedConnector;
        }

        public void UpdateFeed(IEnumerable<Channel> channels)
        {
            _log.LogInformation("Started updating channels");

            foreach (var channel in channels)
            {
                try
                {
                    UpdateChannelAsync(channel.Id).Wait();
                }
                catch (Exception e)
                {
                    string errorMessage = $"Failed updating feed: {e.Message}";
                    _log.LogError(errorMessage);
                    throw new FeedUpdatingException(errorMessage, e);
                }
            }

            _log.LogInformation("Finished updating channels");
        }

        public async Task UpdateFeedAsync(IEnumerable<Channel> newsSources)
        {
            _log.LogInformation("Started updating channels");

            foreach (var newsSource in newsSources)
            {
                try
                {
                    await UpdateChannelAsync(newsSource.Id);
                }
                catch (Exception e)
                {
                    string errorMessage = $"Failed updating feed: {e.Message}";
                    _log.LogError(errorMessage);
                    throw new FeedUpdatingException(errorMessage, e);
                }
            }

            _log.LogInformation("Finished updating channels");
        }

        public async Task UpdateChannelAsync(int channelId)
        {
            var channel = _channelDataService.GetById(channelId);
            if (channel.IsUpdating)
            {
                _log.LogError($"Channel {channelId} is currently being updated");
                return;
            }

            try
            {
                SetChannelUpdatingState(channel, true);
                var news = await _feedConnector.GetFeed(channel.FeedUrl, GetchannelFeedFormat(channel));
                SaveFeed(channel.Id, news);
                SetChannelUpdatingState(channel, false);
            }
            catch (Exception e)
            {
                string errorMessage = $"Failed updating channel {channelId}: {e.Message}";

                if(e is EntityNotFoundException)
                {
                    errorMessage = $"Failed updating the feed: channel with id {channelId} does not exist";
                }

                _log.LogError(errorMessage);
                SetChannelUpdatingState(channel, false);
                throw new FeedUpdatingException(errorMessage, e);
            }
        }

        public void UpdateChannel(int channelId)
        {
            var channel = _channelDataService.GetById(channelId);
            if (channel.IsUpdating)
            {
                _log.LogInformation($"Channel {channelId} is currently being updated");
                return;
            }

            try
            {
                SetChannelUpdatingState(channel, true);
                var news = _feedConnector.GetFeed(channel.FeedUrl, channel.FeedFormat).Result;
                SaveFeed(channel.Id, news);
                SetChannelUpdatingState(channel, false);
            }
            catch (Exception e)
            {
                string errorMessage = $"Failed updating channel {channelId}: {e.Message}";

                if(e is EntityNotFoundException)
                {
                    errorMessage = $"Failed updating the feed: channel with id {channelId} does not exist";
                }

                _log.LogError(errorMessage);
                SetChannelUpdatingState(channel, false);
                throw new FeedUpdatingException(errorMessage, e);
            }
        }

        public async Task<Channel> AddFeedChannel(string feedUrl, bool isPrivate, int userId)
        {
            if (string.IsNullOrEmpty(feedUrl))
            {
                throw new ArgumentNullException(nameof(feedUrl), "Feed url cannot be null or empty");
            }

            try
            {
                var channelModel = await _feedConnector.GetFeedSource(feedUrl);
                var channel = new Channel
                {
                    Name = channelModel.Name?
                            .RemoveTabulation(" ")
                            .RemoveHtmlTags()
                            .RemoveNonAlphanumericCharacters()
                            .CropString(100) ?? "Untitled",
                    Description = channelModel.Description?
                            .RemoveTabulation(" ")
                            .RemoveHtmlTags()
                            .RemoveNonAlphanumericCharacters()
                            .CropString(255),
                    ImageUrl = channelModel.ImageUrl,
                    FeedUrl = channelModel.FeedUrl,
                    WebsiteUrl = channelModel.WebsiteUrl,
                    FeedFormat = channelModel.FeedFormat,
                    Language = channelModel.Language
                };

                if(channelModel.UpdateIntervalMinutes != 0)
                {
                    channel.UpdateIntervalMinutes = channelModel.UpdateIntervalMinutes;
                }

                var addedChannel = _channelDataService.Add(channel);
                UpdateChannel(addedChannel.Id);
                _channelDataService.SubscribeUser(addedChannel.Id, userId, isPrivate);
                
                return addedChannel;
            }
            catch (Exception e)
            {
                throw new FeedUpdatingException($"Failed to get RSS {feedUrl} channel info", e);
            }
        }

        private void SetChannelUpdatingState(Channel channel, bool isUpdating)
        {
            channel.IsUpdating = isUpdating;
            // If finished updating -> update the DateFeedUpdated property
            if(!isUpdating)
            {
                channel.DateFeedUpdated = DateTime.UtcNow;
            }
            _channelDataService.Update(channel);
        }

        private void SaveFeed(int channelId, List<FeedItemModel> feed)
        {
            foreach (var feedItem in feed)
            {
                FeedItem existingFeedItem;
                if (!_feedDataService.Exists(feedItem.Id))
                {
                    try
                    {
                        existingFeedItem = _feedDataService.Add(new FeedItem()
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
                        _log.LogError($"Failed to add a feed item {feedItem.Id}", e);
                        continue;
                    }
                }
                else 
                {
                    existingFeedItem = _feedDataService.GetByGuid(feedItem.Id);
                }

                _feedDataService.Update(existingFeedItem.Id, channelId, feedItem.Categories);
            }
        }

        private FeedFormat GetchannelFeedFormat(Channel channel)
        {
            return (FeedFormat)((int)channel.FeedFormat);
        }
    }
}