using System.Collections.Generic;
using System.Threading.Tasks;
using NewsParser.DAL.Models;

namespace NewsParser.FeedParser.Services
{
    /// <summary>
    /// Interface contains a declaration of methods for updating the RSS feed
    /// </summary>
    public interface IFeedUpdater
    {
        /// <summary>
        /// Updates all feed
        /// </summary>
        /// <param name="channels">Channels to update</param>
        void UpdateFeed(IEnumerable<Channel> channels);

        /// <summary>
        /// Updates all feed (async mode)
        /// </summary>
        /// <param name="channels">Channels to update</param>
        /// <returns>Task</returns>
        Task UpdateFeedAsync(IEnumerable<Channel> channels);

        /// <summary>
        /// Updates a single channel
        /// </summary>
        /// <param name="sourceId">Channel id</param>
        /// <returns></returns>
        void UpdateChannel(int sourceId);

        /// <summary>
        /// Updates a single channel (async)
        /// </summary>
        /// <param name="sourceId">Channel id</param>
        /// <returns></returns>
        Task UpdateChannelAsync(int sourceId);

        /// <summary>
        /// Adds the channel
        /// </summary>
        /// <param name="feedUrl">Feed url</param>
        /// <param name="isPrivate">Indicates whether this channel is private</param>
        /// <param name="userId">User id to add channel to</param>
        /// <returns>Channel object</returns>
        Task<Channel> AddFeedChannel(string feedUrl, bool isPrivate, int userId);
    }
}
