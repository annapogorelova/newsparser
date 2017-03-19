using System.Collections.Generic;
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
        /// <returns>IEnumerable of NewsSource</returns>
        IEnumerable<NewsSource> GetNewsSources(bool hasUsers = false);

        /// <summary>
        /// Gets news sources that user is subscribed to
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>IEnumerable of NewsSource</returns>
        IEnumerable<NewsSource> GetNewsSourcesByUser(int userId);

            /// <summary>
        /// Get news sources that are available for the user specified
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>IEnumerable of NewsSource</returns>
        IEnumerable<NewsSource> GetAvailableNewsSources(int userId);

        /// <summary>
        /// Get a page of available news sources
        /// </summary>
        /// <param name="total">Total count of filtered news sources</param>
        /// <param name="search">Search term</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="subscribed">If true and userId present only user subscribed sources will be returned, otherwise - all</param>
        /// <param name="userId">User id to select available news sources for</param>
        /// <returns>IEnumerable of NewsSource</returns>
        IEnumerable<NewsSource> GetNewsSourcesPage(out int total, int pageIndex = 0, int pageSize = 5, string search = null, bool subscribed = false, int? userId = null);

        /// <summary>
        /// Get news source by id
        /// </summary>
        /// <param name="id">News source id</param>
        /// <returns>NewsSource object</returns>
        NewsSource GetNewsSourceById(int id);

        /// <summary>
        /// Get news source by RSS url
        /// </summary>
        /// <param name="rssUrl">News source rss url</param>
        /// <returns>NewsSource object</returns>
        NewsSource GetNewsSourceByUrl(string rssUrl);

        /// <summary>
        /// Insert a news source
        /// </summary>
        /// <param name="newsSource">NewsSource object</param>
        /// <returns>NewsSource object</returns>
        NewsSource AddNewsSource(NewsSource newsSource);

        /// <summary>
        /// Adds a news source to user
        /// </summary>
        /// <param name="sourceId">News source id</param>
        /// <param name="userId">User id</param>
        void AddNewsSourceToUser(int sourceId, int userId);

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

        /// <summary>
        /// Delete news source from user
        /// </summary>
        /// <param name="sourceId">News source id</param>
        /// <param name="userId">User id</param>
        void DeleteUserNewsSource(int sourceId, int userId);
    }
}
