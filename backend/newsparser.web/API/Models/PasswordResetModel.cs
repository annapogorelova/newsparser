using System.ComponentModel.DataAnnotations;

namespace NewsParser.API.Models
{
    /// <summary>
    /// Class contains a properties for the password reset
    /// </summary>
    public class PasswordResetModel
    {
        [Required(ErrorMessage = "Reset password token is required")]
        public string PasswordResetToken { get; set; }

        [Required(ErrorMessage = "New password is required")]
        public string NewPassword { get; set; }
    }
}