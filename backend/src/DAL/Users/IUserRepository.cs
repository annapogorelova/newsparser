using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Users
{
    public interface IUserRepository
    {
        User GetUserById(int id);
        User GetUserByEmail(string email);
        IQueryable<User> GetUses();
        User AddUser(User user);
        User UpdateUser(User user);
        void DeleteUser(int id);
    }
}
