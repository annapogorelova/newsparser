using System;
using NewsParser.DAL.Models;

namespace NewsParser.FeedParser.Models
{
    public class ChannelModel
    {
        public string Name { get; set; }
        public string FeedUrl { get; set; }
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }
        public string ImageUrl { get; set; }
        public FeedFormat FeedFormat { get; set; }
        public int UpdateIntervalMinutes { get; set; }
        public string Language { get; set; }
    }
}