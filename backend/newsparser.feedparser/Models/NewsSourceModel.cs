using System;

namespace NewsParser.FeedParser.Models
{
    public class NewsSourceModel
    {
        public string Name { get; set; }
        public string RssUrl { get; set; }
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? LastBuildDate { get; set; }
    }
}