using System;
using System.Collections.Generic;

namespace NewsParser.API.Models
{
    /// <summary>
    /// Class represents the NewsItem entity for API
    /// </summary>
    public class NewsItemApiModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string LinkToSource { get; set; }
        public string ImageUrl { get; set; }
        public NewsSourceApiModel Source { get; set; }        
        public List<string> Tags { get; set; } 
    }
}
