using System.Collections.Generic;
using NewsParser.DAL.Models;

namespace NewsParser.BL.Services.Channels
{
    /// <summary>
    /// Provides a business layer functionality for Channel data access
    /// </summary>
    public interface IChannelDataService
    {
        /// <summary>
        /// Get all channels, including the private ones
        /// </summary>
        /// <param name="hasUsers">By default false; 
        /// If true - query will include only channels with at least one subscribed user</param>
        /// <returns>IEnumerable of Channel</returns>
        IEnumerable<Channel> GetAll(bool hasUsers = false);

        /// <summary>
        /// Get channels to be updated
        /// </summary>
        /// <returns>IEnumerable of Channel</returns>
        IEnumerable<Channel> GetForUpdate();

        /// <summary>
        /// Gets channels that user is subscribed to
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>IEnumerable of Channel</returns>
        IEnumerable<Channel> GetByUser(int userId);

        /// <summary>
        /// Determines, whether user is already subscribed to this channel
        /// </summary>
        /// <param name="channelId">Channel id</param>
        /// <param name="userId">User id</param>
        /// <returns>True if user is subscribed, false - if not</returns>
        bool IsUserSubscribed(int channelId, int userId);

        /// <summary>
        /// Get a page of available channels
        /// </summary>
        /// <param name="userId">User id to select available channels for</param>
        /// <param name="total">Total count of filtered channels</param>
        /// <param name="search">Search term</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="subscribed">If true and userId present only user subscribed channels will be returned, otherwise - all</param>
        /// <returns>IEnumerable of Channel</returns>
        IEnumerable<Channel> GetPage(
            int userId, 
            out int total, 
            int pageIndex = 0, 
            int pageSize = 5, 
            string search = null, 
            bool subscribed = false
        );

        /// <summary>
        /// Get channel by id
        /// </summary>
        /// <param name="id">Channel id</param>
        /// <returns>Channel object</returns>
        Channel GetById(int id);

        /// <summary>
        /// Get channel by RSS url
        /// </summary>
        /// <param name="feedUrl">Channel feed url</param>
        /// <returns>Channel object</returns>
        Channel GetByUrl(string feedUrl);

        /// <summary>
        /// Insert a channel
        /// </summary>
        /// <param name="channel">Channel object</param>
        /// <returns>Channel object</returns>
        Channel Add(Channel channel);

        /// <summary>
        /// Adds a channel to user
        /// </summary>
        /// <param name="channelId">Channel id</param>
        /// <param name="userId">User id</param>
        /// <param name="isPrivate">Indicates whether the source should be private</param>
        void SubscribeUser(int channelId, int userId, bool isPrivate = false);

        /// <summary>
        /// Updates a channel
        /// </summary>
        /// <param name="channel">Channel object</param>
        void Update(Channel channel);

        /// <summary>
        /// Deletes channel
        /// </summary>
        /// <param name="Channel">Channel object</param>
        void Delete(Channel channel);

        /// <summary>
        /// Delete channel from user
        /// </summary>
        /// <param name="channelId">Channel id</param>
        /// <param name="userId">User id</param>
        void DeleteUserChannel(int channelId, int userId);

        /// <summary>
        /// Unsubscribes user from the public source or removes the source if it's private to this user
        /// </summary>
        /// <param name="channelId">Channel id</param>
        /// <param name="userId">User id</param>
        void UnsubscribeUser(int channelId, int userId);

        /// <summary>
        /// Determines whether the channel is in the list of user's private sources
        /// </summary>
        /// <param name="channelId">Channel id</param>
        /// <param name="userId">User id</param>
        /// <returns></returns>
        bool IsPrivateToUser(int channelId, int userId);

        /// <summary>
        /// Determines whether the user can delete the source
        /// </summary>
        /// <param name="channelId">Channel id</param>
        /// <param name="userId">User id</param>
        /// <returns></returns>
        bool CanDelete(int channelId, int userId);

        /// <summary>
        /// Determines whether this channel is visible for this user
        /// </summary>
        /// <param name="channelId">Channel id</param>
        /// <param name="userId">User id</param>
        /// <returns>true if visible, false if not</returns>
        bool IsVisibleToUser(int channelId, int userId);
    }
}
