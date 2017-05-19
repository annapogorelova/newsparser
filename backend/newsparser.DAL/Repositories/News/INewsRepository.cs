using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Repositories.News
{
    /// <summary>
    /// Provides a functionality to access the NewsItem entity data
    /// </summary>
    public interface INewsRepository
    {
        /// <summary>
        /// Get all news items
        /// </summary>
        /// <returns>IQueryable of NewsItem</returns>
        IQueryable<NewsItem> GetNews();

        /// <summary>
        /// Get the page of news
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="userId">User id. If present, only the news of this user will be fetched</param>
        /// <param name="search">Search term (Title or Description)</param>
        /// <param name="sourcesIds">The array of sources ids to get news by</param>
        /// <param name="tags">The array of tags to get news by</param>
        /// <returns></returns>
        IQueryable<NewsItem> GetNewsPage(
            int pageIndex = 0, 
            int pageSize = 5,
            int? userId = null, 
            string search = null,
            int[] sourcesIds = null, 
            string[] tags = null
        );

        /// <summary>
        /// Get news by the user id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>IQueryable of NewsItem</returns>
        IQueryable<NewsItem> GetNewsByUser(int userId);

        /// <summary>
        /// Get news items by source id
        /// </summary>
        /// <param name="sourceId">Source id</param>
        /// <returns>IQueryable of NewsItem</returns>
        IQueryable<NewsItem> GetNewsBySource(int sourceId);

        /// <summary>
        /// Gets new items by tag name
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns>IQueryable of NewsItem</returns>
        IQueryable<NewsItem> GetNewsByTagName(string tagName);

        /// <summary>
        /// Gets news items by tag id
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <returns>IQueryable of NewsItem</returns>
        IQueryable<NewsItem> GetNewsByTagId(int tagId);

        /// <summary>
        /// Gets news item by id
        /// </summary>
        /// <param name="id">News item id</param>
        /// <returns>NewsItem object</returns>
        NewsItem GetNewsById(int id);

        /// <summary>
        /// Gets news item by the link to it's source
        /// </summary>
        /// <param name="linkToSource">Link to source string</param>
        /// <returns>NewsItem object</returns>
        NewsItem GetNewsItemByLink(string linkToSource);

        /// <summary>
        /// Gets news item by the guid
        /// </summary>
        /// <param name="linkToSource">Guid string</param>
        /// <returns>NewsItem object</returns>
        NewsItem GetNewsItemByGuid(string guid);

        /// <summary>
        /// Inserts a news item
        /// </summary>
        /// <param name="newsItem">NewsItem object</param>
        /// <returns>Inserted NewsItem object</returns>
        NewsItem AddNewsItem(NewsItem newsItem);

        /// <summary>
        /// Inserts a list of news items
        /// </summary>
        /// <param name="newsItems">IEnumerable of NewsItem</param>
        void AddNewsItems(IEnumerable<NewsItem> newsItems);

        /// <summary>
        /// Inserts a NewsTagNews record that connects news item and tag
        /// </summary>
        /// <param name="newsItemId">News item id</param>
        /// <param name="tagId">Tag id</param>
        void AddNewsItemTag(int newsItemId, int tagId);

        /// <summary>
        /// Adds a source to news item
        /// </summary>
        /// <param name="newsItemId">News item id</param>
        /// <param name="sourceId">Source id</param>
        void AddNewsItemSource(int newsItemId, int sourceId);
        
        /// <summary>
        /// Deletes a news item
        /// </summary>
        /// <param name="newsItem">NewsItem object</param>
        void DeleteNewsItem(NewsItem newsItem);

        /// <summary>
        /// Deletes a range of news
        /// </summary>
        void DeleteNews(IEnumerable<NewsItem> news);

        /// <summary>
        /// Deterrmines whether news item has a source with the given id
        /// </summary>
        /// <param name="newsItemId">News item id</param>
        /// <param name="sourceId">Source id</param>
        /// <returns>True if news item has such news source, false - if not</returns>
        bool NewsItemHasSource(int newsItemId, int sourceId);
    }
}
