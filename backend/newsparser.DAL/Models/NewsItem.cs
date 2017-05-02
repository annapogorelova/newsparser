using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsParser.DAL.Models
{
    public class NewsItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(255)]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(255)]
        public string LinkToSource { get; set; }
        
        public int SourceId { get; set; }
        public NewsSource Source { get; set; }

        public List<NewsTagsNews> NewsItemTags { get; set; }

        public DateTime DatePublished { get; set; }

        public DateTime DateAdded { get; set; }  = DateTime.UtcNow;
    }
}
