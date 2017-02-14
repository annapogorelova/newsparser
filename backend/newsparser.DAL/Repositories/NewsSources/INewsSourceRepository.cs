using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Repositories.NewsSources
{
    /// <summary>
    /// Provides functionality for accessing the news sources data
    /// </summary>
    public interface INewsSourceRepository
    {
        /// <summary>
        /// Gets news sources
        /// </summary>
        /// <returns>IQueryable of NewsSource</returns>
        IQueryable<NewsSource> GetNewsSources();

        /// <summary>
        /// Gets news sources by user id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>IQueryable of NewsSource</returns>
        IQueryable<NewsSource> GetNewsSourcesByUser(int userId);

        /// <summary>
        /// Gets news source by id
        /// </summary>
        /// <param name="id">News source id</param>
        /// <returns>NewsSource object</returns>
        NewsSource GetNewsSourceById(int id);

        /// <summary>
        /// Get news source by RSS url
        /// </summary>
        /// <param name="rssUrl">RSS url</param>
        /// <returns>NewsSource object</returns>
        NewsSource GetNewsSourceByUrl(string rssUrl);

        /// <summary>
        /// Inserts a news source
        /// </summary>
        /// <param name="newsSource">NewsSource object</param>
        void AddNewsSource(NewsSource newsSource);

        /// <summary>
        /// Updates a news source
        /// </summary>
        /// <param name="newsSource">NewsSource object</param>
        void UpdateNewsSource(NewsSource newsSource);

        /// <summary>
        /// Deletes a news source
        /// </summary>
        /// <param name="newsSource">NewsSource object</param>
        void DeleteNewsSource(NewsSource newsSource);
    }
}
