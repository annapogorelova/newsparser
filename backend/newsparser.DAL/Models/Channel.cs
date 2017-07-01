using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsParser.DAL.Models
{
    public class Channel
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

        [Column(TypeName = "CHAR(2)")]
        public string Language { get; set; }
        
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        
        public DateTime DateFeedUpdated { get; set; }

        [Column(TypeName = "TINYINT")]
        public FeedFormat FeedFormat { get; set; }

        public int UpdateIntervalMinutes { get; set; } = DALConstants.DefaultFeedUpdateIntervalMinutes;
        
        public List<ChannelFeedItem> Feed { get; set; }

        public List<UserChannel> Users { get; set; } = new List<UserChannel>();
    }

    public enum FeedFormat
    {
        RSS = 1,
        Atom = 2
    }
}
