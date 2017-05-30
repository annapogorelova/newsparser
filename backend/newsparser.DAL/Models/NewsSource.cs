using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsParser.DAL.Models
{
    public class NewsSource
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }
        
        [Required, MaxLength(255)]
        public string FeedUrl { get; set; }
        
        [MaxLength(255)]
        public string ImageUrl { get; set; }
        
        [MaxLength(255)]
        public string WebsiteUrl { get; set; }
        
        [MaxLength(255)]
        public string Description { get; set; }
        
        public bool IsUpdating { get; set; }
        
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        
        public DateTime DateFeedUpdated { get; set; }

        public FeedFormat FeedFormat { get; set; }

        public int UpdateIntervalMinutes { get; set; }
        
        public List<NewsSourceNews> NewsSources { get; set; }

        public List<UserNewsSource> UsersSources { get; set; } = new List<UserNewsSource>();
    }

    public enum FeedFormat
    {
        RSS = 1,
        Atom = 2
    }
}
