using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using newsparser.DAL.Models;

namespace NewsParser.DAL.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [Required, MaxLength(int.MaxValue)]
        public string Password { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; set; }
        
        public List<UserNewsSource> NewsSources { get; set; }

        public List<UserSocialId> UserSocialIds { get; set; }
    }
}
