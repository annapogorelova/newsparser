using System.ComponentModel.DataAnnotations;

namespace NewsParser.API.Models
{
    /// <summary>
    /// Class contains properties for creating a user
    /// </summary>
    public class CreateAccountModel
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}