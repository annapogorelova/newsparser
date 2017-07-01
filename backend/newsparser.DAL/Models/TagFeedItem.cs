namespace NewsParser.DAL.Models
{
    public class TagFeedItem
    {
        public int Id { get; set; }

        public int FeedItemId { get; set; }
        public FeedItem FeedItem { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
