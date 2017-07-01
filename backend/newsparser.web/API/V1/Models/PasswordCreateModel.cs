using System.ComponentModel.DataAnnotations;

namespace NewsParser.API.V1.Models
{
    public class PasswordCreateModel
    {
        [Required(ErrorMessage = "Password is required"), MinLength(8)]
        public string Password { get; set; }
    }
}