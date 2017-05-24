using System.ComponentModel.DataAnnotations;

namespace NewsParser.API.Models
{
    /// <summary>
    /// Class represents a news source to be created
    /// </summary>
    public class NewsSourceCreateModel
    {
        [MaxLength(255)]
        [Url(ErrorMessage = "RSS url must be a valid fully-qualified http or https URL string")]
        [Required(ErrorMessage = "RSS url is required")]
        public string RssUrl { get; set; }
    }
}
