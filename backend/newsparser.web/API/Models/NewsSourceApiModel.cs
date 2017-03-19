namespace NewsParser.API.Models
{
    /// <summary>
    /// Class represent a NewsSource model to be passed over API
    /// </summary>
    public class NewsSourceApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RssUrl { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
