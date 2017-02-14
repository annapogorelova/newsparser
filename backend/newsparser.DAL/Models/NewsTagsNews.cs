namespace NewsParser.DAL.Models
{
    public class NewsTagsNews
    {
        public int Id { get; set; }

        public int NewsItemId { get; set; }
        public NewsItem NewsItem { get; set; }

        public int TagId { get; set; }
        public NewsTag Tag { get; set; }
    }
}
