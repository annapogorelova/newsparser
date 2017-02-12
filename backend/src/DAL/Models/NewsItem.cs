using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsParser.DAL.Models
{
    public class NewsItem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public string LinkToSource { get; set; }
        
        public int SourceId { get; set; }
        public NewsSource Source { get; set; }

        public List<NewsTagsNews> NewsItemTags { get; set; }
        public List<UserNews> NewsItemUsers { get; set; } 

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}
