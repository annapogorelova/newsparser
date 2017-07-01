using System.ComponentModel.DataAnnotations;
using NewsParser.DAL.Models;

namespace newsparser.DAL.Models
{
    /// <summary>
    /// Social authentication providers
    /// </summary>
    public enum ExternalAuthProvider
    {
        Facebook = 1,
        Google
    }

    public class UserExternalId
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public ExternalAuthProvider AuthProvider { get; set; }

        [Required]
        public string ExternalId { get; set; }

        public User User { get; set; }
    }
}
