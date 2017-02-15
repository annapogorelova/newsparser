namespace NewsParser.API.Models
{
    /// <summary>
    /// Class represents a NewsSource model in API
    /// </summary>
    public class NewsSourceApiListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }
    }
}
