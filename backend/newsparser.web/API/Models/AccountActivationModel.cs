using System.ComponentModel.DataAnnotations;

namespace NewsParser.API.Models
{
    /// <summary>
    /// Class contains properties for activating a user
    /// </summary>
    public class AccountActivationModel
    {       
        [Required]
        public string ConfirmationToken { get; set; }
    }
}