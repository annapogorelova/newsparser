using System;
using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;
using NewsParser.DAL.Repositories.Feed;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.Tags;
using NewsParser.BL.Exceptions;
using NewsParser.DAL.Repositories.Channels;

namespace NewsParser.BL.Services.Feed
{
    /// <summary>
    /// Implementation of IFeedDataService
    /// </summary>
    public class FeedDataService: IFeedDataService
    {
        private readonly IFeedRepository _feedRepository;
        private readonly ITagRepository _tagRepository;

        private readonly IChannelRepository _channelRepository;

        public FeedDataService(
            IFeedRepository feedRepository, 
            ITagRepository tagRepository,
            IChannelRepository channelRepository)
        {
            _feedRepository = feedRepository;
            _tagRepository = tagRepository;
            _channelRepository = channelRepository;
        }

        public IEnumerable<FeedItem> GetPage(
            int pageIndex = 0, 
            int pageSize = 5,
            int? userId = null, 
            string search = null,
            int[] channelsIds = null, 
            string[] tags = null
        )
        {
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "Page index must be greater than 0");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0");
            }

            return _feedRepository.GetPage(pageIndex, pageSize, userId, search, channelsIds, tags);
        }

        public IEnumerable<FeedItem> GetByChannel(int channelId)
        {
            return _feedRepository.GetByChannel(channelId);
        }

        public FeedItem GetById(int id)
        {
            return _feedRepository.GetById(id) ?? 
                throw new EntityNotFoundException("Feed item was not found");
        }

        public FeedItem GetByLink(string linkToSource)
        {
            return _feedRepository.GetByLink(linkToSource) ??
                throw new EntityNotFoundException("Feed item was not found");
        }

        public FeedItem Add(FeedItem feedItem)
        {
            try
            {
                return _feedRepository.Add(feedItem);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException("Failed to add a feed item", e);
            }
        }

        public void AddTag(int feedItemId, int tagId)
        {
            var feedItem = _feedRepository.GetById(feedItemId);
            if (feedItem == null)
            {
                throw new ArgumentException($"Feed item with id {feedItemId} does not exist", nameof(feedItemId));
            }

            var tag = _tagRepository.GetById(tagId);
            if (tag == null)
            {
                throw new ArgumentException($"Tag does with id {tagId} not exist", nameof(tagId));
            }

            try
            {
                _feedRepository.AddTag(feedItemId, tagId);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed adding a tag with id {tagId} to feed item with id {feedItemId}", e);
            }
        }

        public void AddTags(int feedItemId, List<string> tags)
        {
            foreach (var tag in tags)
            {
                var newsTag = _tagRepository.GetByName(tag);
                if(newsTag == null)
                {
                    newsTag = _tagRepository.Add(new Tag { Name = tag.ToLowerInvariant() });
                }

                AddTag(feedItemId, newsTag.Id);
            }
        }

        public void Delete(int id)
        {
            var feedItem = _feedRepository.GetById(id);
            if (feedItem == null)
            {
                throw new BusinessLayerException($"Feed item with id {id} does not exist");
            }

            try
            {
                _feedRepository.Delete(feedItem);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed to delete feed item with id {id}", e);
            }
        }

        public bool Exists(string guid)
        {
            return _feedRepository.GetByGuid(guid) != null;
        }

        public void AddChannel(int feedItemId, int channelId)
        {
            _feedRepository.AddChannel(feedItemId, channelId);
        }

        public FeedItem GetByGuid(string guid)
        {
            return _feedRepository.GetByGuid(guid);
        }

        public void Update(int feedItemId, int channelId, List<string> tags = null)
        {
            try
            {
                if(!_feedRepository.HasChannel(feedItemId, channelId))
                {
                    AddChannel(feedItemId, channelId);
                }

                var feedItemTags = _tagRepository.GetByFeedItemId(feedItemId).ToList();
                var newTags = tags.Except(feedItemTags.Select(t => t.Name)).ToList();

                if(newTags.Any())
                {
                    AddTags(feedItemId, newTags);
                }
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed to update feed item with id {feedItemId}", e);
            }
        }
    }
}
