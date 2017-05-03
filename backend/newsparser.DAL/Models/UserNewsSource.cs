namespace NewsParser.DAL.Models
{
    /// <summary>
    /// Class represents many-to-many relation between User and NewSource entities
    /// </summary>
    public class UserNewsSource
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int SourceId { get; set; }

        public User User { get; set; }
        public NewsSource Source { get; set; }
    }
}
