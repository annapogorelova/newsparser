using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsParser.DAL.Models
{
    public class NewsSourceNews
    {
        public int Id { get; set; }

        public int SourceId { get;set; }

        public NewsSource Source { get; set; }

        public int NewsItemId { get; set; }

        public NewsItem NewsItem { get; set; }
    }
}
