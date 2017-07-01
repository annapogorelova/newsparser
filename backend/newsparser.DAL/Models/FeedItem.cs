using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsParser.DAL.Models
{
    public class FeedItem
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
        
        public List<ChannelFeedItem> Channels { get; set; } = new List<ChannelFeedItem>();

        public List<TagFeedItem> Tags { get; set; } = new List<TagFeedItem>();
    }
}
