using System.ComponentModel.DataAnnotations;
using NewsParser.DAL.Models;

namespace newsparser.DAL.Models
{
    /// <summary>
    /// Social authentication providers
    /// </summary>
    public enum SocialAuthProvider
    {
        Facebook = 1,
        Google
    }

    public class UserSocialId
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public SocialAuthProvider AuthProvider { get; set; }

        [Required]
        public string SocialId { get; set; }

        public User User { get; set; }
    }
}
