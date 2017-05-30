using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Repositories.NewsSources
{
    /// <summary>
    /// Provides functionality for accessing the news sources data
    /// </summary>
    public class NewsSourceRepository: INewsSourceRepository
    {
        private readonly AppDbContext _dbContext;

        public NewsSourceRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets news sources
        /// </summary>
        /// <returns>IQueryable of NewsSource</returns>
        public IQueryable<NewsSource> GetNewsSources()
        {
            return _dbContext.NewsSources;
        }

        /// <summary>
        /// Gets news sources by user id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>IQueryable of NewsSource</returns>
        public IQueryable<NewsSource> GetNewsSourcesByUser(int userId)
        {
            return _dbContext.NewsSources
                .Include(s => s.UsersSources)
                .Where(n => n.UsersSources.Any(us => us.UserId == userId));
        }

        /// <summary>
        /// Gets news source by id
        /// </summary>
        /// <param name="id">News source id</param>
        /// <returns>NewsSource object</returns>
        public NewsSource GetNewsSourceById(int id)
        {
            return _dbContext.NewsSources
                .Include(s => s.UsersSources)
                .FirstOrDefault(s => s.Id == id);
        }

        public NewsSource GetNewsSourceByUrl(string rssUrl)
        {
            return _dbContext.NewsSources
                .Include(s => s.UsersSources)
                .FirstOrDefault(n => n.FeedUrl == rssUrl);
        }

        /// <summary>
        /// Inserts a news source
        /// </summary>
        /// <param name="newsSource">NewsSource object</param>
        /// <returns>NewsSource object</returns>
        public NewsSource AddNewsSource(NewsSource newsSource)
        {
            if (newsSource == null)
            {
                throw new ArgumentNullException(nameof(newsSource), "News source cannot be null");
            }

            _dbContext.NewsSources.Add(newsSource);
            _dbContext.SaveChanges();
            return _dbContext.Entry(newsSource).Entity;
        }

        /// <summary>
        /// Updates a news source
        /// </summary>
        /// <param name="newsSource">NewsSource object</param>
        public void UpdateNewsSource(NewsSource newsSource)
        {
            if (newsSource == null)
            {
                throw new ArgumentNullException(nameof(newsSource), "News source cannot be null");
            }

            _dbContext.Entry(newsSource).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Deletes a news source
        /// </summary>
        /// <param name="newsSource">NewsSource object</param>
        public void DeleteNewsSource(NewsSource newsSource)
        {
            if (newsSource == null)
            {
                throw new ArgumentNullException(nameof(newsSource), "News source cannot be null");
            }

            _dbContext.NewsSources.Remove(newsSource);
            _dbContext.SaveChanges();
        }

        public void AddNewsSourceToUser(UserNewsSource userNewsSource)
        {
            _dbContext.UserSources.Add(userNewsSource);
            _dbContext.SaveChanges();
        }

        public void DeleteUserNewsSource(int sourceId, int userId)
        {
            var userNewsSource =
                _dbContext.UserSources.FirstOrDefault(us => us.SourceId == sourceId && us.UserId == userId);
            if (userNewsSource == null)
            {
                throw new NullReferenceException($"UserNewsSource with user id {userId} and source id {sourceId} does not exist");
            }

            _dbContext.UserSources.Remove(userNewsSource);
            _dbContext.SaveChanges();
        }

        public IQueryable<UserNewsSource> GetSourceUsers(int sourceId)
        {
            return _dbContext.UserSources.Where(s => s.SourceId == sourceId);
        }

        public bool IsSourceVisibleToUser(int sourceId, int userId)
        {
            return !_dbContext.UserSources.Any(us => us.SourceId == sourceId) ||
                !_dbContext.UserSources.Where(us => us.SourceId == sourceId)
                    .All(us => us.UserId != userId  && us.IsPrivate);
        }

        public IQueryable<NewsSource> GetUsersPrivateNewsSources(int userId)
        {
            return _dbContext.NewsSources
                .Include(s => s.UsersSources)
                .Where(s => s.UsersSources.Any(us => us.UserId == userId && us.IsPrivate));
        }
    }
}
