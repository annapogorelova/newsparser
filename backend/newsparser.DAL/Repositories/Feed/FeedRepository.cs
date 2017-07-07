using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Repositories.Feed
{
    /// <summary>
    /// Repository for accessing the feed data
    /// </summary>
    public class FeedRepository : IFeedRepository
    {
        private readonly AppDbContext _dbContext;

        public FeedRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<FeedItem> GetAll()
        {
            return _dbContext.FeedItems;
        }

        public IQueryable<FeedItem> GetPage(
            int pageIndex = 0, 
            int pageSize = 5,
            int? userId = null, 
            string search = null,
            int[] channelsIds = null, 
            string[] tags = null
        )
        {
            return _dbContext.FeedItems.Where(n => 
                    (string.IsNullOrEmpty(search) 
                    || n.Title.Contains(search) 
                    || n.Description.Contains(search))
                    && (channelsIds == null || n.Channels.Any(s => channelsIds.Contains(s.ChannelId)))
                    && (userId == null || n.Channels.Select(s => s.Channel).Any(s => s.Users.Any(u => u.UserId == userId)))
                    && (tags == null || n.Tags.Any(t => tags.Contains(t.Tag.Name))))
                .OrderByDescending(n => n.DatePublished)
                .Skip(pageIndex)
                .Take(pageSize)
                .Include(n => n.Channels)
                .ThenInclude(ns => ns.Channel)
                .Include(n => n.Tags)
                .ThenInclude(t => t.Tag)
                .AsNoTracking();
        }

        public IQueryable<FeedItem> GetByUser(int userId)
        {
            return _dbContext.FeedItems
                    .Include(n => n.Channels)
                    .ThenInclude(s => s.Channel)
                    .Where(n => n.Channels.Select(s => s.Channel)
                    .Any(s => s.Users.Any(u => u.UserId == userId)));
        }

        public IQueryable<FeedItem> GetByChannel(int sourceId)
        {
            return _dbContext.FeedItems.Where(n => n.Channels.Any(s => s.ChannelId == sourceId));
        }

        public IQueryable<FeedItem> GetByTag(string tagName)
        {
            return _dbContext.FeedItems.Where(n => n.Tags.Any(t => t.Tag.Name == tagName));
        }

        public IQueryable<FeedItem> GetByTagId(int tagId)
        {
            return _dbContext.FeedItems.Where(n => n.Tags.Any(t => t.Tag.Id == tagId));
        }

        public FeedItem GetById(int id)
        {
            return _dbContext.FeedItems.Find(id);
        }

        public FeedItem GetByLink(string linkToSource)
        {
            return _dbContext.FeedItems.FirstOrDefault(n => n.LinkToSource == linkToSource);
        }

        public FeedItem Add(FeedItem newsItem)
        {
            if (newsItem == null)
            {
                throw new ArgumentNullException(nameof(newsItem), "Feed can not be null");
            }

            _dbContext.FeedItems.Add(newsItem);
            _dbContext.SaveChanges();
            return _dbContext.Entry(newsItem).Entity;
        }

        public void Add(IEnumerable<FeedItem> newsItems)
        {
            if (newsItems == null)
            {
                throw new ArgumentNullException(nameof(newsItems), "Feed can not be null");
            }
            _dbContext.FeedItems.AddRange(newsItems);
            _dbContext.SaveChanges();
        }

        public void AddTag(int feedItemId, int tagId)
        {
            _dbContext.TagFeedItems.Add(new TagFeedItem { FeedItemId = feedItemId, TagId = tagId });
            _dbContext.SaveChanges();
        }

        public void Delete(FeedItem newsItem)
        {
            if (newsItem == null)
            {
                throw new ArgumentNullException(nameof(newsItem), "Feed item can not be null");
            }

            _dbContext.FeedItems.Remove(newsItem);
            _dbContext.SaveChanges();
        }

        public void Delete(IEnumerable<FeedItem> feed)
        {
            if (feed == null)
            {
                throw new ArgumentNullException(nameof(feed), "Feed cannot be null");
            }

            _dbContext.FeedItems.RemoveRange(feed);
            _dbContext.SaveChanges();
        }

        public void AddChannel(int newsItemId, int sourceId)
        {
            _dbContext.ChannelFeedItems.Add(new ChannelFeedItem() {
                FeedItemId = newsItemId,
                ChannelId = sourceId
            });
            _dbContext.SaveChanges();
        }

        public FeedItem GetByGuid(string guid)
        {
            return _dbContext.FeedItems.FirstOrDefault(n => n.Guid == guid);
        }

        public bool HasChannel(int feedItemId, int channelId)
        {
            return _dbContext.ChannelFeedItems
                .Any(ns => ns.FeedItemId == feedItemId && ns.ChannelId == channelId);
        }

        public void Update(FeedItem feedItem)
        {
            if (feedItem == null)
            {
                throw new ArgumentNullException(nameof(feedItem), "Feed item cannot be null");
            }

            _dbContext.Entry(feedItem).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
