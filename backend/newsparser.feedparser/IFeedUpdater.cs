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
        /// <param name="sourceId">Source to update id. If specified, only this source will be updated</param>
        void UpdateFeed(int? userId = null, int? sourceId = null);

        /// <summary>
        /// Updates all feed (async mode)
        /// </summary>
        /// <param name="userId">User id. If specified only this user sources will be updated</param>
        /// <param name="sourceId">Source to update id.If specified, only this source will be updated</param>
        /// <returns>Task</returns>
        Task UpdateFeedAsync(int? userId = null, int? sourceId = null);

        /// <summary>
        /// Addes the news source by RSS url
        /// </summary>
        /// <param name="rssUrl">RSS url</param>
        /// <param name="userId">User id to add news source to</param>
        /// <returns>NewsSource object</returns>
        Task<NewsSource> AddNewsSource(string rssUrl, int? userId = null);
    }
}
