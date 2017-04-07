using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore.Extensions;
using newsparser.DAL.Models;
using NewsParser.DAL.Models;

namespace NewsParser.DAL
{
    public class AppDbContext: DbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<NewsItem> News { get; set; }
        public DbSet<NewsSource> NewsSources { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }
        public DbSet<NewsTagsNews> NewsTagsNews { get; set; }
        public DbSet<UserNewsSource> UserSources { get; set; }
        public DbSet<UserExternalId> UserExternalIds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;userid=root;pwd=tolochko;port=3306;database=news_parser_db;sslmode=none;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");

            modelBuilder.Entity<UserNewsSource>().ToTable("user_sources");

            modelBuilder.Entity<UserNewsSource>()
                .HasOne(ns => ns.User)
                .WithMany(u => u.NewsSources)
                .HasForeignKey(ns => ns.UserId);

            modelBuilder.Entity<UserNewsSource>()
                .HasOne(ns => ns.NewsSource)
                .WithMany(u => u.Users)
                .HasForeignKey(ns => ns.SourceId);

            modelBuilder.Entity<UserExternalId>().ToTable("user_external_ids");

            modelBuilder.Entity<UserExternalId>()
                .HasOne(s => s.User)
                .WithMany(u => u.UserExternalIds)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<NewsItem>().ToTable("news");

            modelBuilder.Entity<NewsItem>()
                .HasOne(n => n.Source)
                .WithMany(s => s.News)
                .HasForeignKey(n => n.SourceId);

            modelBuilder.Entity<NewsTag>().ToTable("news_tags");

            modelBuilder.Entity<NewsTagsNews>().ToTable("news_tags_news");

            modelBuilder.Entity<NewsTagsNews>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.TagNewsItems)
                .HasForeignKey(nt => nt.TagId);
            modelBuilder.Entity<NewsTagsNews>()
                .HasOne(nt => nt.NewsItem)
                .WithMany(t => t.NewsItemTags)
                .HasForeignKey(nt => nt.NewsItemId);

            modelBuilder.Entity<NewsSource>().ToTable("news_sources");
        }
    }
}
