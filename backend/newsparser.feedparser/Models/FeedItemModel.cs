using System;
using System.Collections.Generic;

namespace NewsParser.FeedParser.Models
{
    public class FeedItemModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DatePublished { get; set; }
        public string LinkToSource { get; set; }
        public string Id { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Categories { get; set; }
    }

    public class RssItemGuid 
    {
        public string GuidString { get; set; }
        public bool IsPermaLink { get; set; } = true;
    }
}
