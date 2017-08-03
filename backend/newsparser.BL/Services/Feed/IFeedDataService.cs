using System;
using System.Collections.Generic;
using NewsParser.DAL.Models;

namespace NewsParser.BL.Services.Feed
{
    /// <summary>
    /// Provides a business layer functionality for News data access
    /// </summary>
    public interface IFeedDataService
    {
        /// <summary>
        /// Get a page of feed
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="channelsIds">Channels ids to select by</param>
        /// <param name="userId">User id</param>
        /// <param name="search">Search string</param>
        /// <param name="tags">Tags names to select by</param>
        /// <returns>IEnumerable of FeedItem</returns>
        IEnumerable<FeedItem> GetPage(
            int pageIndex = 0, 
            int pageSize = 5, 
            int? userId = null, 
            string search = null,
            int[] channelsIds = null, 
            string[] tags = null
        );

        /// <summary>
        /// Get feed by channel
        /// </summary>
        /// <param name="channelId">Channel id</param>
        /// <returns>IEnumerable of FeedItem</returns>
        IEnumerable<FeedItem> GetByChannel(int channelId);

        /// <summary>
        /// Get feed item by id
        /// </summary>
        /// <param name="id">Feed item id</param>
        /// <returns>FeedItem object</returns>
        FeedItem GetById(int id);

        /// <summary>
        /// Get feed item by the link to channel
        /// </summary>
        /// <param name="linkToChannel">Link to channel string</param>
        /// <returns>FeedItem object</returns>
        FeedItem GetByLink(string linkToChannel);

        /// <summary>
        /// Get feed item by the guid
        /// </summary>
        /// <param name="linkToChannel">Guid string</param>
        /// <returns>FeedItem object</returns>
        FeedItem GetByGuid(string guid);

        /// <summary>
        /// Inserts a feed item
        /// </summary>
        /// <param name="feedItem">FeedItem object</param>
        /// <returns>FeedItem object</returns>
        FeedItem Add(FeedItem feedItem);
        
        /// <summary>
        /// Adds a tag to feed item
        /// </summary>
        /// <param name="feedItemId">Feed item id</param>
        /// <param name="tagId">Tag id</param>
        void AddTag(int feedItemId, int tagId);

        /// <summary>
        /// Add list of tags to feed item
        /// </summary>
        /// <param name="feedItemId">Feed item id</param>
        /// <param name="tags">List of tags (string)</param>
        void AddTags(int feedItemId, List<string> tags);

        /// <summary>
        /// Add an existing channel to the feed item
        /// </summary>
        /// <param name="feedItemId">Feed item id</param>
        /// <param name="channelId">Channel id</param>
        void AddChannel(int feedItemId, int channelId);

        /// <summary>
        /// Deletes a feed item
        /// </summary>
        /// <param name="id">Feed item id</param>
        void Delete(int id);

        /// <summary>
        /// Checks if feed item with the indicated guid already exists
        /// </summary>
        /// <param name="guid">Guid string</param>
        /// <returns>True if exists, false if not</returns>
        bool Exists(string guid);

        /// <summary>
        /// Updates the feed item properties
        /// </summary>
        /// <param name="feedItem">FeedItem object</param>
        void Update(FeedItem feedItem);

        /// <summary>
        /// Update the list of tags of the feed item
        /// </summary>
        /// <param name="feedItemId">Feed item id</param>
        /// <param name="tags">List of tags</param>
        void UpdateTags(int feedItemId, List<string> tags = null);

        /// <summary>
        /// Determines whether the feed item already has the channel specified
        /// </summary>
        /// <param name="feedItemId">Feed item id</param>
        /// <param name="channelId">Channel id</param>
        /// <returns>True is has, false - if not</returns>
        bool HasChannel(int feedItemId, int channelId);

        /// <summary>
        /// Adds or updates feed item
        /// </summary>
        /// <param name="feedItem">Feed item</param>
        /// <param name="channelId">Channel id</param>
        /// <param name="tags">List of tags</param>
        void AddOrUpdate(FeedItem feedItem, int channelId, List<string> tags);
    }
}
