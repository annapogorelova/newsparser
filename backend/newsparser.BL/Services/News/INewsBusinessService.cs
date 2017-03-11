using System.Collections.Generic;
using NewsParser.DAL.Models;

namespace NewsParser.BL.Services.News
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
        /// <param name="sourcesIds">Sources ids to select by</param>
        /// <param name="userId">User id</param>
        /// <param name="search">Search string</param>
        /// <param name="tags">Tags names to select by</param>
        /// <returns>IEnumerable of NewsItem</returns>
        IEnumerable<NewsItem> GetNewsPage(int pageIndex = 0, int pageSize = 5, 
            int? userId = null, string search = null, int[] sourcesIds = null, string[] tags = null);

        /// <summary>
        /// Get news by source
        /// </summary>
        /// <param name="sourceId">Source id</param>
        /// <returns>IEnumerable of NewsItem</returns>
        IEnumerable<NewsItem> GetNewsBySource(int sourceId);

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
