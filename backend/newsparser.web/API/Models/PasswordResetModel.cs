using System.ComponentModel.DataAnnotations;

namespace NewsParser.API.Models
{
    /// <summary>
    /// Class contains a properties for the password reset
    /// </summary>
    public class PasswordResetModel
    {
        [Required]
        public string PasswordResetToken { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}