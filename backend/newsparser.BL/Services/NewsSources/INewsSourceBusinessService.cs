﻿using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.BL.Services.NewsSources
{
    /// <summary>
    /// Provides a business layer functionality for NewsSource data access
    /// </summary>
    public interface INewsSourceBusinessService
    {
        /// <summary>
        /// Get all news sources
        /// </summary>
        /// <param name="hasUsers">By default false; 
        /// If true - query will include only news sources with at least one subscribed user</param>
        /// <returns>IQueryable of NewsSource</returns>
        IQueryable<NewsSource> GetNewsSources(bool hasUsers = false);

        /// <summary>
        /// Get news source by user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>IQueryable of news sources</returns>
        IQueryable<NewsSource> GetUserNewsSources(int userId);

        /// <summary>
        /// Get news source by id
        /// </summary>
        /// <param name="id">News source id</param>
        /// <returns>NewsSource object</returns>
        NewsSource GetNewsSourceById(int id);

        /// <summary>
        /// Insert a news source
        /// </summary>
        /// <param name="newsSource">NewsSource object</param>
        void AddNewsSource(NewsSource newsSource);

        /// <summary>
        /// Updates a news source
        /// </summary>
        /// <param name="newsSource">NewsSource object</param>
        void UpdateNewsSource(NewsSource newsSource);

        /// <summary>
        /// Deletes news source
        /// </summary>
        /// <param name="id">News source id</param>
        void DeleteNewsSource(int id);
    }
}