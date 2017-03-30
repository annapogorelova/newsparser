using System.Linq;
using newsparser.DAL.Models;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Repositories.Users
{
    /// <summary>
    /// Provides a functionality to access the User entity data
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User object</returns>
        User GetUserById(int id);

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns>User object</returns>
        User GetUserByEmail(string email);

        /// <summary>
        /// Get user by social id
        /// </summary>
        /// <param name="socialId">User social id</param>
        /// <param name="provider">Social authentication provider (Facebook/Google+)</param>
        /// <returns>User object</returns>
        User GetUserBySocialId(string socialId, ExternalAuthProvider provider);

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>IQueryable of User</returns>
        IQueryable<User> GetUsers();

        /// <summary>
        /// Get users by source id
        /// </summary>
        /// <param name="sourceId">Source id</param>
        /// <returns>IQueryable of User</returns>
        IQueryable<User> GetUsersByNewsSource(int sourceId);

        /// <summary>
        /// Insert a user
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns>User object</returns>
        User AddUser(User user);

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="user">User object</param>
        void UpdateUser(User user);

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="user">User object</param>
        void DeleteUser(User user);
    }
}
