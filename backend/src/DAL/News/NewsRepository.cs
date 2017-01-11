using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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
        public IQueryable<NewsItem> GetNews(int startIndex = 0, int numResults = 5, int? categoryId = null, int? sourceId = null)
        {
            IQueryable<NewsItem> newsBaseQuery = _dbContext.News.Include(n => n.Category).Include(n => n.Source);

            if (categoryId != null)
            {
                newsBaseQuery = newsBaseQuery.Where(n => n.CategoryId == categoryId.Value);
            }

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
        /// Gets news by category
        /// </summary>
        /// <param name="categoryId">Category id</param>
        /// <returns>IQueryable of NewsItem</returns>
        public IQueryable<NewsItem> GetNewsByCategory(int categoryId)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets news by source
        /// </summary>
        /// <param name="sourceId">Source id</param>
        /// <returns>IQueryable of NewsItem</returns>
        public IQueryable<NewsItem> GetNewsBySource(int sourceId)
        {
            throw new System.NotImplementedException();
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
    }
}
