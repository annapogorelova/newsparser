using System;
using System.Collections.Generic;

namespace newsparser.feedparser
{
    public class NewsItemParseModel
    {
        public string Title { get; set; } = "Untitled";
        public string Description { get; set; }
        public DateTime DatePublished { get; set; } = DateTime.UtcNow;
        public string LinkToSource { get; set; }
        public RssItemGuid Guid { get; set; }
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
