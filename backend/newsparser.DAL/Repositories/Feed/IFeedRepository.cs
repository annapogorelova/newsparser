using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Repositories.Feed
{
    /// <summary>
    /// Provides a functionality to access the FeedItem entity data
    /// </summary>
    public interface IFeedRepository
    {
        /// <summary>
        /// Get all feed items
        /// </summary>
        /// <returns>IQueryable of FeedItem</returns>
        IQueryable<FeedItem> GetAll();

        /// <summary>
        /// Get the page of news
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="userId">User id. If present, only the news of this user will be fetched</param>
        /// <param name="search">Search term (Title or Description)</param>
        /// <param name="channelsIds">The array of channels ids to get news by</param>
        /// <param name="tags">The array of tags to get news by</param>
        /// <returns></returns>
        IQueryable<FeedItem> GetPage(
            int pageIndex = 0, 
            int pageSize = 5,
            int? userId = null, 
            string search = null,
            int[] channelsIds = null, 
            string[] tags = null
        );

        /// <summary>
        /// Get news by the user id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>IQueryable of FeedItem</returns>
        IQueryable<FeedItem> GetByUser(int userId);

        /// <summary>
        /// Get feed items by channel id
        /// </summary>
        /// <param name="channelId">channel id</param>
        /// <returns>IQueryable of FeedItem</returns>
        IQueryable<FeedItem> GetByChannel(int channelId);

        /// <summary>
        /// Gets new items by tag name
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns>IQueryable of FeedItem</returns>
        IQueryable<FeedItem> GetByTag(string tagName);

        /// <summary>
        /// Gets feed items by tag id
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <returns>IQueryable of FeedItem</returns>
        IQueryable<FeedItem> GetByTagId(int tagId);

        /// <summary>
        /// Gets feed item by id
        /// </summary>
        /// <param name="id">feed item id</param>
        /// <returns>FeedItem object</returns>
        FeedItem GetById(int id);

        /// <summary>
        /// Gets feed item by the link to it's channel
        /// </summary>
        /// <param name="linkTochannel">Link to channel string</param>
        /// <returns>FeedItem object</returns>
        FeedItem GetByLink(string linkTochannel);

        /// <summary>
        /// Gets feed item by the guid
        /// </summary>
        /// <param name="linkTochannel">Guid string</param>
        /// <returns>FeedItem object</returns>
        FeedItem GetByGuid(string guid);

        /// <summary>
        /// Inserts a feed item
        /// </summary>
        /// <param name="FeedItem">FeedItem object</param>
        /// <returns>Inserted FeedItem object</returns>
        FeedItem Add(FeedItem FeedItem);

        /// <summary>
        /// Inserts a list of feed items
        /// </summary>
        /// <param name="FeedItems">IEnumerable of FeedItem</param>
        void Add(IEnumerable<FeedItem> FeedItems);

        /// <summary>
        /// Inserts a NewsTagNews record that connects feed item and tag
        /// </summary>
        /// <param name="feedItemId">feed item id</param>
        /// <param name="tagId">Tag id</param>
        void AddTag(int feedItemId, int tagId);

        /// <summary>
        /// Adds a channel to feed item
        /// </summary>
        /// <param name="feedItemId">feed item id</param>
        /// <param name="channelId">channel id</param>
        void AddChannel(int feedItemId, int channelId);
        
        /// <summary>
        /// Deletes a feed item
        /// </summary>
        /// <param name="feedItem">FeedItem object</param>
        void Delete(FeedItem feedItem);

        /// <summary>
        /// Deletes a range of news
        /// </summary>
        void Delete(IEnumerable<FeedItem> feed);

        /// <summary>
        /// Deterrmines whether feed item has a channel with the given id
        /// </summary>
        /// <param name="feedItemId">feed item id</param>
        /// <param name="channelId">channel id</param>
        /// <returns>True if feed item has such news channel, false - if not</returns>
        bool HasChannel(int feedItemId, int channelId);
    }
}
