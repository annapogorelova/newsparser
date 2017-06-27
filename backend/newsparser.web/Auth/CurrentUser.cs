using NewsParser.Web.Identity.Models;

namespace NewsParser.Web.Auth
{
    public static class CurrentUser
    {
        private static ApplicationUser _user;
        public static void SetCurrentUser(ApplicationUser user)
        {
            _user = user;
        }

        public static ApplicationUser GetCurrentUser()
        {
            return _user;
        }
    }
}