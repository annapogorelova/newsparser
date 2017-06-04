using Microsoft.EntityFrameworkCore;
using newsparser.DAL.Models;
using NewsParser.DAL.Models;

namespace NewsParser.DAL
{
    public class AppDbContext: DbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<FeedItem> FeedItems { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagFeedItem> TagFeedItems { get; set; }
        public DbSet<ChannelFeedItem> ChannelFeedItems { get; set; }
        public DbSet<UserChannel> UserChannels { get; set; }
        public DbSet<UserExternalId> UserExternalIds { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;userid=root;pwd=tolochko;port=3306;database=news_parser_db;sslmode=none;charset=utf8;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");

            modelBuilder.Entity<UserChannel>().ToTable("user_channels");

            modelBuilder.Entity<UserChannel>()
                .HasOne(us => us.User)
                .WithMany(u => u.Channels)
                .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<UserChannel>()
                .HasOne(us => us.Channel)
                .WithMany(c => c.Users)
                .HasForeignKey(us => us.ChannelId);

            modelBuilder.Entity<UserExternalId>().ToTable("user_external_ids");

            modelBuilder.Entity<UserExternalId>()
                .HasOne(ue => ue.User)
                .WithMany(u => u.UserExternalIds)
                .HasForeignKey(ue => ue.UserId);

            modelBuilder.Entity<FeedItem>()
                .ToTable("feed_items")
                .HasIndex(f => f.LinkToSource);

            modelBuilder.Entity<ChannelFeedItem>().ToTable("channels_feed_items");
            
            modelBuilder.Entity<ChannelFeedItem>()
                .HasOne(cf => cf.Channel)
                .WithMany(c => c.Feed)
                .HasForeignKey(cf => cf.ChannelId);
            modelBuilder.Entity<ChannelFeedItem>()
                .HasOne(cf => cf.FeedItem)
                .WithMany(f => f.Channels)
                .HasForeignKey(cf => cf.FeedItemId);

            modelBuilder.Entity<Tag>()
                .ToTable("tags")
                .HasIndex(t => t.Name);

            modelBuilder.Entity<TagFeedItem>().ToTable("tags_feed_items");

            modelBuilder.Entity<TagFeedItem>()
                .HasOne(tf => tf.Tag)
                .WithMany(t => t.Feed)
                .HasForeignKey(tf => tf.TagId);
            modelBuilder.Entity<TagFeedItem>()
                .HasOne(tf => tf.FeedItem)
                .WithMany(t => t.Tags)
                .HasForeignKey(tf => tf.FeedItemId);

            modelBuilder.Entity<Channel>().ToTable("channels");
            modelBuilder.Entity<Channel>().HasIndex(s => s.FeedFormat);
            modelBuilder.Entity<Channel>().HasIndex(s => s.FeedUrl);
            modelBuilder.Entity<Channel>().HasIndex(s => s.Language);

            modelBuilder.Entity<Token>().ToTable("tokens");
        }
    }
}
