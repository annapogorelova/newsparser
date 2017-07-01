using System.ComponentModel.DataAnnotations;
using NewsParser.Helpers.ValidationAttributes;

namespace NewsParser.API.V1.Models
{
    /// <summary>
    /// Class contains a properties for the password change
    /// </summary>
    public class PasswordChangeModel
    {
        [Required(ErrorMessage = "Current password is required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required"), MinLength(8)]
        [NotEqual(PropertyName = "CurrentPassword", 
            ErrorMessage = "New password must be different from the current password")]
        public string NewPassword { get; set; }
    }
}