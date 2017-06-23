using System.ComponentModel.DataAnnotations;

namespace NewsParser.API.Models
{
    public class AccountModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        public bool HasPassword { get; set; }
    }
}
