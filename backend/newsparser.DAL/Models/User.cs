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
        
        public List<UserNewsSource> Sources { get; set; }

        public List<UserExternalId> UserExternalIds { get; set; }

        public List<NewsSource> CreatedNewsSources { get; set; }

        public User()
        {
            Sources = new List<UserNewsSource>();
            UserExternalIds = new List<UserExternalId>();
        }
    }
}
