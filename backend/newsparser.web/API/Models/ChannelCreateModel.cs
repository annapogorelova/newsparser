using System.ComponentModel.DataAnnotations;

namespace NewsParser.API.Models
{
    /// <summary>
    /// Class represents a channel to be created
    /// </summary>
    public class ChannelCreateModel
    {
        [MaxLength(255)]
        [Url(ErrorMessage = "Feed url must be a valid fully-qualified http or https URL string")]
        [Required(ErrorMessage = "Feed url is required")]
        public string FeedUrl { get; set; }

        public bool IsPrivate { get; set; } = false;
    }
}
