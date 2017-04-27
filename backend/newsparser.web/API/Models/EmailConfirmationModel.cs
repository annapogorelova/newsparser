using System.ComponentModel.DataAnnotations;

namespace NewsParser.API.Models
{
    /// <summary>
    /// Class contains properties for the user's email conformation
    /// </summary>
    public class EmailConfirmationModel
    {       
        [Required(ErrorMessage = "Email confirmation token is required")]
        public string ConfirmationToken { get; set; }
    }
}