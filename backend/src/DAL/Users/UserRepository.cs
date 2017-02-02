using System.Linq;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Users
{
    public class UserRepository: IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User GetUserById(int id)
        {
            return _dbContext.Users.Find(id);
        }

        public User GetUserByEmail(string email)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        }

        public IQueryable<User> GetUses()
        {
            return _dbContext.Users;
        }

        public void AddUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public void UpdateUser(User updatedUser)
        {
            _dbContext.Entry(updatedUser).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

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
