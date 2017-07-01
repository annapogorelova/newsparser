namespace NewsParser.DAL.Models
{
    /// <summary>
    /// Class represents many-to-many relation between User and Channel entities
    /// </summary>
    public class UserChannel
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int ChannelId { get; set; }

        public bool IsPrivate { get; set; }

        public User User { get; set; }
        public Channel Channel { get; set; }
    }
}
