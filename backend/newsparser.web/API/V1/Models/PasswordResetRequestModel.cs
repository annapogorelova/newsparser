using System.ComponentModel.DataAnnotations;

namespace NewsParser.API.V1.Models
{
    /// <summary>
    /// Class contains a properties for the password reset request
    /// </summary>
    public class PasswordResetRequestModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}