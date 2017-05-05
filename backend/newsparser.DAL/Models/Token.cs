using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using newsparser.DAL.Models;

namespace NewsParser.DAL.Models
{
    public class Token
    {
        public string Id { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Type { get; set; }

        public Token()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
