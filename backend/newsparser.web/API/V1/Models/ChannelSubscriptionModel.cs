namespace NewsParser.API.V1.Models
{
    /// <summary>
    /// Class represent a NewsSource model to be passed over API
    /// </summary>
    public class ChannelSubscriptionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FeedUrl { get; set; }
        public string ImageUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string Description { get; set; }
        public bool IsSubscribed { get; set; }
        public bool IsPrivate { get; set; }
        public int SubscribersCount { get; set; }
        public string Language { get; set; }
    }
}