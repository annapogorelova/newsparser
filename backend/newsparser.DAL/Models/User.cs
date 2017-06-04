using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using newsparser.DAL.Models;

namespace NewsParser.DAL.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(36)]
        public string UserName {get; set;} = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required, MaxLength(int.MaxValue)]
        public string Password { get; set; }

        public bool EmailConfirmed { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
        
        public List<UserChannel> Channels { get; set; }

        public List<UserExternalId> UserExternalIds { get; set; }

        public List<Channel> CreatedChannels { get; set; }

        public User()
        {
            Channels = new List<UserChannel>();
            UserExternalIds = new List<UserExternalId>();
        }
    }
}
