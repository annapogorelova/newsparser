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

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(255)]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(255)]
        public string Guid { get; set; }

        [MaxLength(100)]
        public string Author { get; set; }

        [Required]
        [MaxLength(255)]
        public string LinkToSource { get; set; }

        public DateTime DatePublished { get; set; } = DateTime.UtcNow;

        public DateTime DateAdded { get; set; }  = DateTime.UtcNow;
        
        public List<NewsSourceNews> Sources { get; set; } = new List<NewsSourceNews>();

        public List<NewsTagsNews> Tags { get; set; } = new List<NewsTagsNews>();
    }
}
