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
        /// Gets the news sources that need updating now
        /// </summary>
        /// <returns>IQueryable of NewsSource</returns>
        IQueryable<NewsSource> GetNewsSourcesByUpdateDate();

        /// <summary>
        /// Gets news sources by user id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>IQueryable of NewsSource</returns>
        IQueryable<NewsSource> GetNewsSourcesByUser(int userId);

        /// <summary>
        /// Gets the user's private news sources
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>IQueryable of NewsSource</returns>
        IQueryable<NewsSource> GetUsersPrivateNewsSources(int userId);

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
        /// <returns>NewsSource object</returns>
        NewsSource AddNewsSource(NewsSource newsSource);

        /// <summary>
        /// Get users subscribed to this source
        /// </summary>
        /// <param name="sourceId">Source id</param>
        /// <returns>IQueryable of UserNewsSource</returns>
        IQueryable<UserNewsSource> GetSourceUsers(int sourceId);

        /// <summary>
        /// Determines whether the source is visible to user
        /// </summary>
        /// <param name="sourceId">Source id</param>
        /// <param name="userId">User id</param>
        /// <returns>True if visible, false - if not</returns>
        bool IsSourceVisibleToUser(int sourceId, int userId);

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

        /// <summary>
        /// Adds a news source to user
        /// </summary>
        /// <param name="userNewsSource">User news source relation object</param>
        void AddNewsSourceToUser(UserNewsSource userNewsSource);

        /// <summary>
        /// Delete news source from user
        /// </summary>
        /// <param name="sourceId">News source id</param>
        /// <param name="userId">User id</param>
        void DeleteUserNewsSource(int sourceId, int userId);
    }
}
