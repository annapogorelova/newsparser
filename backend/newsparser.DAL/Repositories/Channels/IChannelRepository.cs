using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Repositories.Channels
{
    /// <summary>
    /// Provides functionality for accessing the channels data
    /// </summary>
    public interface IChannelRepository
    {
        /// <summary>
        /// Gets channels
        /// </summary>
        /// <returns>IQueryable of Channel</returns>
        IQueryable<Channel> GetAll();

        /// <summary>
        /// Gets the channels that need updating now
        /// </summary>
        /// <returns>IQueryable of Channel</returns>
        IQueryable<Channel> GetByUpdateDate();

        /// <summary>
        /// Gets channels by user id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>IQueryable of Channel</returns>
        IQueryable<Channel> GetByUser(int userId);

        /// <summary>
        /// Gets the user's private channels
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>IQueryable of Channel</returns>
        IQueryable<Channel> GetPrivateByUser(int userId);

        /// <summary>
        /// Gets channel by id
        /// </summary>
        /// <param name="id">Channel id</param>
        /// <returns>Channel object</returns>
        Channel GetById(int id);

        /// <summary>
        /// Get channel by RSS url
        /// </summary>
        /// <param name="feedUrl">RSS url</param>
        /// <returns>Channel object</returns>
        Channel GetByUrl(string feedUrl);

        /// <summary>
        /// Inserts a channel
        /// </summary>
        /// <param name="channel">Channel object</param>
        /// <returns>Channel object</returns>
        Channel Add(Channel channel);

        /// <summary>
        /// Get users subscribed to this source
        /// </summary>
        /// <param name="channelId">Source id</param>
        /// <returns>IQueryable of UserChannel</returns>
        IQueryable<UserChannel> GetUsers(int channelId);

        /// <summary>
        /// Determines whether the source is visible to user
        /// </summary>
        /// <param name="channelId">Source id</param>
        /// <param name="userId">User id</param>
        /// <returns>True if visible, false - if not</returns>
        bool IsVisibleToUser(int channelId, int userId);

        /// <summary>
        /// Updates a channel
        /// </summary>
        /// <param name="channel">Channel object</param>
        void Update(Channel channel);

        /// <summary>
        /// Deletes a channel
        /// </summary>
        /// <param name="channel">Channel object</param>
        void Delete(Channel channel);

        /// <summary>
        /// Adds a channel to user
        /// </summary>
        /// <param name="userChannel">User channel relation object</param>
        void AddUser(UserChannel userChannel);

        /// <summary>
        /// Delete channel from user
        /// </summary>
        /// <param name="channelId">Channel id</param>
        /// <param name="userId">User id</param>
        void DeleteUserChannel(int channelId, int userId);
    }
}
