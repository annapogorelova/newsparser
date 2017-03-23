using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NewsParser.Identity
{
    /// <summary>
    /// Application user class
    /// </summary>
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int GetId()
        {
            return int.Parse(Id);
        }
    }
}
