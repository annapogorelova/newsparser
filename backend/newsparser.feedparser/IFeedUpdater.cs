﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NewsParser.DAL.Models;

namespace newsparser.feedparser
{
    /// <summary>
    /// Interface contains a declaration of methods for updating the RSS feed
    /// </summary>
    public interface IFeedUpdater
    {
        /// <summary>
        /// Updates all feed
        /// </summary>
        /// <param name="newsSources">News sources to update</param>
        void UpdateFeed(IEnumerable<NewsSource> newsSources);

        /// <summary>
        /// Updates all feed (async mode)
        /// </summary>
        /// <param name="newsSources">News sources to update</param>
        /// <returns>Task</returns>
        Task UpdateFeedAsync(IEnumerable<NewsSource> newsSources);

        /// <summary>
        /// Updates a single news source
        /// </summary>
        /// <param name="sourceId">Source id</param>
        /// <returns></returns>
        void UpdateSource(int sourceId);

        /// <summary>
        /// Updates a single news source (async)
        /// </summary>
        /// <param name="sourceId">Source id</param>
        /// <returns></returns>
        Task UpdateSourceAsync(int sourceId);

        /// <summary>
        /// Adds the news source by RSS url
        /// </summary>
        /// <param name="rssUrl">RSS url</param>
        /// <param name="userId">User id to add news source to</param>
        /// <returns>NewsSource object</returns>
        Task<NewsSource> AddNewsSource(string rssUrl, int? userId = null);
    }
}
