using System.ComponentModel.DataAnnotations;

namespace NewsParser.API.Models
{
    /// <summary>
    /// Class contains a properties for the password reset request
    /// </summary>
    public class PasswordResetRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}