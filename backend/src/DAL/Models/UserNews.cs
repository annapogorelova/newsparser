
namespace NewsParser.DAL.Models
{
    /// <summary>
    /// Class represents a many-to-many relation between User and NewsItem entities
    /// </summary>
    public class UserNews
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int NewsItemId { get; set; }

        public User User { get; set; }
        public NewsItem NewsItem { get; set; }
    }
}
