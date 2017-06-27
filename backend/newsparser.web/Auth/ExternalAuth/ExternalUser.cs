using newsparser.DAL.Models;

namespace NewsParser.Web.Auth.ExternalAuth
{
    public class ExternalUser
    {
        public string ExternalId { get; set; }
        public ExternalAuthProvider AuthProvider { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
    }
}
