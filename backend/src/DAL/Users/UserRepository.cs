using System.Linq;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Users
{
    /// <summary>
    /// Repository for accessing the news user data
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
        /// <param name="id">User's id</param>
        /// <returns>User object</returns>
        public User GetUserById(int id)
        {
            return _dbContext.Users.Find(id);
        }

        /// <summary>
        /// Gets user by email
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>User object</returns>
        public User GetUserByEmail(string email)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        }

        /// <summary>
        /// Gets the current user
        /// </summary>
        /// <returns>User object</returns>
        public User GetCurrentUser()
        {
            // Will be changed when there is proper authentication
            return _dbContext.Users.First();
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
        /// Inserts a new user
        /// </summary>
        /// <param name="user">User object</param>
        public void AddUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="updatedUser">User object</param>
        public void UpdateUser(User updatedUser)
        {
            _dbContext.Entry(updatedUser).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="id">User id</param>
        public void DeleteUser(int id)
        {
            var userToDelete = _dbContext.Users.Find(id);
            if(userToDelete != null)
            {
                _dbContext.Remove(userToDelete);
                _dbContext.SaveChanges();
            }
        }
    }
}
