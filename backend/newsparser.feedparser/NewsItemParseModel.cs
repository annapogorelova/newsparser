using System;
using System.Collections.Generic;

namespace newsparser.feedparser
{
    public class NewsItemParseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DatePublished { get; set; }
        public string LinkToSource { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Categories { get; set; }
    }
}
