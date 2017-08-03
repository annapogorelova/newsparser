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
using AutoMapper;
using NewsParser.FeedParser.Extensions;

namespace NewsParser.FeedParser.Services
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
                UpdateChannel(channel.Id);
            }

            _log.LogInformation("Finished updating channels");
        }

        public async Task UpdateFeedAsync(IEnumerable<Channel> newsSources)
        {
            _log.LogInformation("Started updating channels");

            foreach (var newsSource in newsSources)
            {
                await UpdateChannelAsync(newsSource.Id);
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
                var feed = await _feedConnector.ParseFeed(channel.FeedUrl, GetChannelFeedFormat(channel));
                SaveFeed(channel.Id, feed);
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
                var feed = _feedConnector.ParseFeed(channel.FeedUrl, channel.FeedFormat).Result;
                SaveFeed(channel.Id, feed);
                SetChannelUpdatingState(channel, false);
            }
            catch (BusinessLayerException e)
            {
                string errorMessage = $"Failed updating channel {channelId}: {e.Message}";
                _log.LogError(errorMessage);
                SetChannelUpdatingState(channel, false);
                throw new FeedUpdatingException(errorMessage, e);
            }
            catch(EntityNotFoundException e)
            {
                string errorMessage = $"Failed updating the feed: channel with id {channelId} does not exist";
                _log.LogError(errorMessage);
                SetChannelUpdatingState(channel, false);
                throw new FeedUpdatingException(errorMessage, e);
            }
            catch(Exception e)
            {
                throw new FatalFeedUpdatingException($"Fatal error happened when updating channel width id {channel.Id}", e);
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
                var channelModel = await _feedConnector.ParseFeedSource(feedUrl);
                var channel = AutoMapper.Mapper.Map<ChannelModel, Channel>(channelModel);
                var addedChannel = _channelDataService.Add(channel);
                UpdateChannel(addedChannel.Id);
                _channelDataService.SubscribeUser(addedChannel.Id, userId, isPrivate);
                
                return addedChannel;
            }
            catch (Exception e)
            {
                _log.LogError($"Failed to add a feed channel with feed url {feedUrl}");
                throw e;
            }
        }

        private void SetChannelUpdatingState(Channel channel, bool isUpdating)
        {
            channel.IsUpdating = isUpdating;
            if(!isUpdating)
            {
                channel.DateFeedUpdated = DateTime.UtcNow;
            }
            _channelDataService.Update(channel);
        }

        private void SaveFeed(int channelId, List<FeedItemModel> feed)
        {
            var nonUpdatadbleProperties = new string[] {"Id", "Channels", "Tags", "DateAdded", "DatePublished" }; 
            foreach (var feedItemModel in feed)
            {
                try
                {
                    var feedItemToAdd = AutoMapper.Mapper.Map<FeedItemModel, FeedItem>(feedItemModel);
                    _feedDataService.AddOrUpdate(feedItemToAdd, channelId, feedItemModel.Categories);
                }
                catch(BusinessLayerException e)
                {
                    _log.LogError($"Failed to add or update the feed item with guid {feedItemModel.Id}. Error: {e.Message}");
                }
            }
        }

        private FeedFormat GetChannelFeedFormat(Channel channel)
        {
            return (FeedFormat)((int)channel.FeedFormat);
        }
    }
}