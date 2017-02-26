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
        /// <param name="userId">User id. If specified only this user sources will be updated</param>
        void UpdateFeed(int? userId = null);

        /// <summary>
        /// Updates all feed (async mode)
        /// </summary>
        /// <param name="userId">User id. If specified only this user sources will be updated</param>
        /// <returns>Task</returns>
        Task UpdateFeedAsync(int? userId = null);

        /// <summary>
        /// Updates a single news source
        /// </summary>
        /// <param name="sourceId">Source id</param>
        /// <param name="userId">User id</param>
        /// <returns></returns>
        void UpdateSource(int sourceId, int? userId = null);

        /// <summary>
        /// Updates a single news source (async)
        /// </summary>
        /// <param name="sourceId">Source id</param>
        /// <param name="userId">User id</param>
        /// <returns></returns>
        Task UpdateSourceAsync(int sourceId, int? userId = null);

        /// <summary>
        /// Adds the news source by RSS url
        /// </summary>
        /// <param name="rssUrl">RSS url</param>
        /// <param name="userId">User id to add news source to</param>
        /// <returns>NewsSource object</returns>
        Task<NewsSource> AddNewsSource(string rssUrl, int? userId = null);
    }
}
