using NewsParser.Helpers.ValidationAttributes;

namespace NewsParser.API.Models
{
    /// <summary>
    /// Model contains a news GET request parameters
    /// </summary>
    public class NewsListSelectModel
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 5;

        public string Search { get; set; }

        [DigitsStringArray]
        public string[] Sources { get; set; } = null;

        public string[] Tags { get; set; } = null;
    }
}
