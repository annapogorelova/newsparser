using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.BL.News
{
    /// <summary>
    /// Provides a business layer functionality for News data access
    /// </summary>
    public interface INewsBusinessService
    {
        /// <summary>
        /// Get a page of news
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="sourceId">Source id</param>
        /// <param name="userId">User id</param>
        /// <returns>IQueryable of NewsItem</returns>
        IQueryable<NewsItem> GetNewsPage(int pageIndex = 0, int pageSize = 5, int? sourceId = null, int? userId = null);

        /// <summary>
        /// Get news by source
        /// </summary>
        /// <param name="sourceId">Source id</param>
        /// <returns>IQueryable of NewsItem</returns>
        IQueryable<NewsItem> GetNewsBySource(int sourceId);

        /// <summary>
        /// Get news item by id
        /// </summary>
        /// <param name="id">News item id</param>
        /// <returns>NewsItem object</returns>
        NewsItem GetNewsItemById(int id);

        /// <summary>
        /// Get news item by the link to source
        /// </summary>
        /// <param name="linkToSource">Link to source string</param>
        /// <returns>NewsItem object</returns>
        NewsItem GetNewsItemByLink(string linkToSource);

        /// <summary>
        /// Inserts a news item
        /// </summary>
        /// <param name="newsItem">NewsItem object</param>
        /// <returns>NewsItem object</returns>
        NewsItem AddNewsItem(NewsItem newsItem);

        /// <summary>
        /// Add a news item to user
        /// </summary>
        /// <param name="newsItemId">News item id</param>
        /// <param name="userId">User id</param>
        void AddNewsItemToUser(int newsItemId, int userId);

        /// <summary>
        /// Add a collection of news to user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="news">List of NewsItem</param>
        void AddNewsToUser(int userId, List<NewsItem> news);

        /// <summary>
        /// Adds a list of news to the source's subscribed users
        /// </summary>
        /// <param name="newsSourceId">News source id</param>
        /// <param name="news">List of NewsItem</param>
        void AddNewsToSource(int newsSourceId, List<NewsItem> news);

        /// <summary>
        /// Adds a tag to news item
        /// </summary>
        /// <param name="newsItemId">News item id</param>
        /// <param name="tagId">Tag id</param>
        void AddTagToNewsItem(int newsItemId, int tagId);

        /// <summary>
        /// Add list of tags to news item
        /// </summary>
        /// <param name="newsItemId">News item id</param>
        /// <param name="tags">List of tags (string)</param>
        void AddTagsToNewsItem(int newsItemId, List<string> tags);

        /// <summary>
        /// Deletes a news item
        /// </summary>
        /// <param name="id">News item id</param>
        void DeleteNewsItem(int id);
    }
}
