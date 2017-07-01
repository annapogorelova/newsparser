using System.ComponentModel.DataAnnotations;

namespace NewsParser.API.V1.Models
{
    /// <summary>
    /// Class contains properties for creating a user
    /// </summary>
    public class CreateAccountModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        
        [MinLength(8)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}