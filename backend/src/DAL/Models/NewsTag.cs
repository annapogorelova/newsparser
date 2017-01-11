using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsParser.DAL.Models
{
    public class NewsTag
    {
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string Name { get; set; }

        public List<NewsTagsNews> NewsTags { get; set; }
    }
}
