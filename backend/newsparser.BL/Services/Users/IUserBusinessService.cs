using System.Collections.Generic;
using newsparser.DAL.Models;
using NewsParser.DAL.Models;

namespace NewsParser.BL.Services.Users
{
    /// <summary>
    /// Provides functionality for accessing the User entity data
    /// </summary>
    public interface IUserBusinessService
    {
        /// <summary>
        /// Get users
        /// </summary>
        /// <returns>IEnumerable of User</returns>
        IEnumerable<User> GetUsers();

        /// <summary>
        /// Get users by news source id
        /// </summary>
        /// <param name="newsSourceId">News source id</param>
        /// <returns>IEnumerable of User</returns>
        IEnumerable<User> GetUsersByNewsSource(int newsSourceId);

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User object</returns>
        User GetUserById(int id);

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>User object</returns>
        User GetUserByEmail(string email);

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="email">UserName</param>
        /// <returns>User object</returns>
        User GetUserByUserName(string userName);

        /// <summary>
        /// Get user by social id
        /// </summary>
        /// <param name="socialId">User social id</param>
        /// <param name="provider">Social authentication provider (Facebook/Google+)</param>
        /// <returns>User object</returns>
        User GetUserBySocialId(string socialId, ExternalAuthProvider provider);

        /// <summary>
        /// Checks if user already exists based on email and user name
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns>True if user exists, false if does not exist</returns>
        bool UserExists(User user);

        /// <summary>
        /// Checks if email is available
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>True if available, false if not available</returns>
        bool EmailAvailable(string email);
    
        /// <summary>
        /// Insert user
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns>User object</returns>
        User AddUser(User user);

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="user">User object</param>
        void UpdateUser(User user);

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id">User id</param>
        void DeleteUser(int id);
    }
}
