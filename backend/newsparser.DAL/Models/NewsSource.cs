using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsParser.DAL.Models
{
    public class NewsSource
    {
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string RssUrl { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public DateTime DateFeedUpdated { get; set; }

        public List<NewsItem> News { get; set; }
        public List<UserNewsSource> Users { get; set; }
    }
}
