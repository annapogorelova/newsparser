using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Repositories.News
{
    /// <summary>
    /// Repository for accessing the news data
    /// </summary>
    public class NewsRepository : INewsRepository
    {
        private readonly AppDbContext _dbContext;

        public NewsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get all news items
        /// </summary>
        /// <returns>IQueryable of NewsItem</returns>
        public IQueryable<NewsItem> GetNews()
        {
            return _dbContext.News.Include(n => n.NewsItemTags).ThenInclude(t => t.Tag);
        }

        /// <summary>
        /// Get news items by source id
        /// </summary>
        /// <param name="sourceId">Source id</param>
        /// <returns>IQueryable of NewsItem</returns>
        public IQueryable<NewsItem> GetNewsBySource(int sourceId)
        {
            return _dbContext.News.Where(n => n.SourceId == sourceId);
        }

        /// <summary>
        /// Gets new items by tag name
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns>IQueryable of NewsItem</returns>
        public IQueryable<NewsItem> GetNewsByTagName(string tagName)
        {
            return _dbContext.News.Include(n => n.NewsItemTags)
                .Where(n => n.NewsItemTags.Any(t => t.Tag.Name == tagName));
        }

        /// <summary>
        /// Gets news items by tag id
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <returns>IQueryable of NewsItem</returns>
        public IQueryable<NewsItem> GetNewsByTagId(int tagId)
        {
            return _dbContext.News.Include(n => n.NewsItemTags)
                .Where(n => n.NewsItemTags.Any(t => t.Tag.Id == tagId));
        }

        /// <summary>
        /// Gets news item by id
        /// </summary>
        /// <param name="id">News item id</param>
        /// <returns>NewsItem object</returns>
        public NewsItem GetNewsById(int id)
        {
            return _dbContext.News.Find(id);
        }

        /// <summary>
        /// Gets news item by the link to it's source
        /// </summary>
        /// <param name="linkToSource">Link to source string</param>
        /// <returns>NewsItem object</returns>
        public NewsItem GetNewsItemByLink(string linkToSource)
        {
            return _dbContext.News.FirstOrDefault(n => n.LinkToSource == linkToSource);
        }

        /// <summary>
        /// Inserts a news item
        /// </summary>
        /// <param name="newsItem">NewsItem object</param>
        /// <returns>Inserted NewsItem object</returns>
        public NewsItem AddNewsItem(NewsItem newsItem)
        {
            if (newsItem == null)
            {
                throw new ArgumentNullException(nameof(newsItem), "News item cannot be null");
            }

            _dbContext.News.Add(newsItem);
            _dbContext.SaveChanges();
            return _dbContext.Entry(newsItem).Entity;
        }

        /// <summary>
        /// Inserts a list of news items
        /// </summary>
        /// <param name="newsItems">IEnumerable of NewsItem</param>
        public void AddNewsItems(IEnumerable<NewsItem> newsItems)
        {
            if (newsItems == null)
            {
                throw new ArgumentNullException(nameof(newsItems), "News items list cannot be null");
            }
            _dbContext.News.AddRange(newsItems);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Inserts a NewsTagNews record that connects news item and tag
        /// </summary>
        /// <param name="newsItemId">News item id</param>
        /// <param name="tagId">Tag id</param>
        public void AddNewsItemTag(int newsItemId, int tagId)
        {
            _dbContext.NewsTagsNews.Add(new NewsTagsNews { NewsItemId = newsItemId, TagId = tagId });
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Deletes a news item
        /// </summary>
        /// <param name="newsItem">NewsItem object</param>
        public void DeleteNewsItem(NewsItem newsItem)
        {
            if (newsItem == null)
            {
                throw new ArgumentNullException(nameof(newsItem), "News item cannot be null");
            }

            _dbContext.News.Remove(newsItem);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Deletes a range of news
        /// </summary>
        public void DeleteNews(IEnumerable<NewsItem> news)
        {
            if (news == null)
            {
                throw new ArgumentNullException(nameof(news), "News collection cannot be null");
            }

            _dbContext.News.RemoveRange(news);
            _dbContext.SaveChanges();
        }
    }
}
