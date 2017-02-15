﻿using System;
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
            return _dbContext.NewsSources.Include(n => n.Users)
                .Where(n => n.Users.Any(u => u.UserId == userId));
        }

        /// <summary>
        /// Gets news source by id
        /// </summary>
        /// <param name="id">News source id</param>
        /// <returns>NewsSource object</returns>
        public NewsSource GetNewsSourceById(int id)
        {
            return _dbContext.NewsSources.Find(id);
        }

        public NewsSource GetNewsSourceByUrl(string rssUrl)
        {
            return _dbContext.NewsSources.FirstOrDefault(n => n.RssUrl == rssUrl);
        }

        /// <summary>
        /// Inserts a news source
        /// </summary>
        /// <param name="newsSource">NewsSource object</param>
        public void AddNewsSource(NewsSource newsSource)
        {
            if (newsSource == null)
            {
                throw new ArgumentNullException(nameof(newsSource), "News source cannot be null");
            }

            _dbContext.NewsSources.Add(newsSource);
            _dbContext.SaveChanges();
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
    }
}