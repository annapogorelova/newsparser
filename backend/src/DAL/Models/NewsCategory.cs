using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsParser.DAL.Models
{
    public class NewsCategory
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<NewsItem> NewsItems { get; set; }
    }
}
