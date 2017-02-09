using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Users
{
    public interface IUserRepository
    {
        User GetUserById(int id);
        User GetUserByEmail(string email);
        IQueryable<User> GetUsers();
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}
