using System;

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
        public string CategoryName { get; set; }
        public string SourceName { get; set; }
    }
}
