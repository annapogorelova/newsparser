using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.News
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
        /// Gets all news from database
        /// </summary>
        /// <returns>IQueryable of NewsItem</returns>
        public IQueryable<NewsItem> GetNews(int startIndex = 0, int numResults = 5, int? sourceId = null)
        {
            IQueryable<NewsItem> newsBaseQuery = _dbContext.News.Include(n => n.Source);

            if (sourceId != null)
            {
                newsBaseQuery = newsBaseQuery.Where(n => n.SourceId == sourceId.Value);
            }

            return newsBaseQuery
                .OrderByDescending(n => n.DateAdded)
                .Skip(startIndex)
                .Take(numResults);
        }

        /// <summary>
        /// Gets news by source
        /// </summary>
        /// <param name="sourceId">Source id</param>
        /// <returns>IQueryable of NewsItem</returns>
        public IQueryable<NewsItem> GetNewsBySource(int sourceId)
        {
            return _dbContext.News.Where(n => n.SourceId == sourceId);
        }

        /// <summary>
        /// Gets news item by Id
        /// </summary>
        /// <param name="id">News item id</param>
        /// <returns>NewsItem object</returns>
        public NewsItem GetNewsById(int id)
        {
            return _dbContext.News.Find(id);
        }

        /// <summary>
        /// Gets news item by link to source
        /// </summary>
        /// <param name="linkToSource">Link to source</param>
        /// <returns>NewsItem object or null if none exists</returns>
        public NewsItem GetNewsItemByLink(string linkToSource)
        {
            return _dbContext.News.FirstOrDefault(n => n.LinkToSource == linkToSource);
        }

        /// <summary>
        /// Inserts news item
        /// </summary>
        /// <param name="newsItem">NewsItem object</param>
        public void AddNewsItem(NewsItem newsItem)
        {
            _dbContext.News.Add(newsItem);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Inserts a range of news items
        /// </summary>
        /// <param name="newsItems">Collection of NewsItem</param>
        public void AddNewsItems(IEnumerable<NewsItem> newsItems)
        {
            _dbContext.News.AddRange(newsItems);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Deletes news item
        /// </summary>
        /// <param name="newsItem">NewsItem object</param>
        public void DeleteNewsItem(int id)
        {
            var newsItemToDelete = _dbContext.News.Find(id);
            if (newsItemToDelete != null)
            {
                _dbContext.News.Remove(newsItemToDelete);
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes news within the dates specified
        /// </summary>
        /// <param name="dateTo">Date to delete news to (required)</param>
        /// <param name="dateFrom">Date to delete news from (not required)</param>
        public void DeleteNews(DateTime dateTo, DateTime? dateFrom = null)
        {
            var newsToDelete =
                _dbContext.News.Where(n => n.DateAdded <= dateTo 
                && (dateFrom == null || n.DateAdded >= dateFrom.Value));

            if (newsToDelete.Any())
            {
                _dbContext.News.RemoveRange(newsToDelete);
                _dbContext.SaveChanges();
            }
        }
    }
}
