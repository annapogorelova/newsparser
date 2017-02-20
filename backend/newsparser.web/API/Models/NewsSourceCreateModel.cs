using System.ComponentModel.DataAnnotations;

namespace NewsParser.API.Models
{
    /// <summary>
    /// Class represents a news source to be created
    /// </summary>
    public class NewsSourceCreateModel
    {
        [Url]
        [MaxLength(100)]
        [Required]
        public string RssUrl { get; set; }
    }
}
