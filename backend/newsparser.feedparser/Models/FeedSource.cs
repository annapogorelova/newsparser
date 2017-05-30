using System;
using NewsParser.DAL.Models;

namespace NewsParser.FeedParser.Models
{
    public class FeedSource
    {
        public string Name { get; set; }
        public string FeedUrl { get; set; }
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? LastBuildDate { get; set; }
        public FeedFormat FeedFormat { get; set; }
    }
}