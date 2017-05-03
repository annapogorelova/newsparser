using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using newsparser.DAL.Models;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Repositories.Users
{
    /// <summary>
    /// Provides a functionality to access the User entity data
    /// </summary>
    public class UserRepository: IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User object</returns>
        public User GetUserById(int id)
        {
            return _dbContext.Users.Find(id);
        }

        /// <summary>
        /// Gets user by email
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns>User object</returns>
        public User GetUserByEmail(string email)
        {
            return _dbContext.Users.Include(u => u.UserExternalIds).FirstOrDefault(u => u.Email == email);
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>IQueryable of User objects</returns>
        public IQueryable<User> GetUsers()
        {
            return _dbContext.Users;
        }

        /// <summary>
        /// Get users by source id
        /// </summary>
        /// <param name="sourceId">Source id</param>
        /// <returns>IQueryable of User</returns>
        public IQueryable<User> GetUsersByNewsSource(int sourceId)
        {
            return
                _dbContext.Users.Include(u => u.Sources)
                    .Where(u => u.Sources.Any(ns => ns.SourceId == sourceId));
        }

        /// <summary>
        /// Inserts a user
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns>User object</returns>
        public User AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return _dbContext.Entry(user).Entity;
        }

        /// <summary>
        /// Updates an user
        /// </summary>
        /// <param name="user">User object</param>
        public void UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            _dbContext.Entry(user).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="user">User object</param>
        public void DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            _dbContext.Remove(user);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Get user by social id
        /// </summary>
        /// <param name="socialId">User social id</param>
        /// <param name="provider">Social authentication provider (Facebook/Google+)</param>
        /// <returns>User object</returns>
        public User GetUserBySocialId(string socialId, ExternalAuthProvider provider)
        {
            return
                _dbContext.Users.Include(u => u.UserExternalIds).FirstOrDefault(
                    u =>
                        u.UserExternalIds.Any(
                            s => s.ExternalId.ToLower() == socialId.ToLower() && s.AuthProvider == provider));
        }

        public User GetUserByUserName(string userName)
        {
            return _dbContext.Users.FirstOrDefault(u => u.UserName == userName);
        }
    }
}
