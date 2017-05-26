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
        public DbSet<NewsItem> News { get; set; }
        public DbSet<NewsSource> NewsSources { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }
        public DbSet<NewsTagsNews> NewsTagsNews { get; set; }
        public DbSet<NewsSourceNews> NewsSourcesNews { get; set; }
        public DbSet<UserNewsSource> UserSources { get; set; }
        public DbSet<UserExternalId> UserExternalIds { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;userid=root;pwd=tolochko;port=3306;database=news_parser_db;sslmode=none;charset=utf8;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");

            modelBuilder.Entity<UserNewsSource>().ToTable("user_news_sources");

            modelBuilder.Entity<UserNewsSource>()
                .HasOne(ns => ns.User)
                .WithMany(u => u.Sources)
                .HasForeignKey(ns => ns.UserId);

            modelBuilder.Entity<UserNewsSource>()
                .HasOne(ns => ns.Source)
                .WithMany(s => s.Users)
                .HasForeignKey(ns => ns.SourceId);

            modelBuilder.Entity<UserExternalId>().ToTable("user_external_ids");

            modelBuilder.Entity<UserExternalId>()
                .HasOne(s => s.User)
                .WithMany(u => u.UserExternalIds)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<NewsItem>()
                .ToTable("news")
                .HasIndex(n => n.LinkToSource);

            modelBuilder.Entity<NewsSourceNews>().ToTable("news_source_news");
            
            modelBuilder.Entity<NewsSourceNews>()
                .HasOne(ns => ns.Source)
                .WithMany(s => s.News)
                .HasForeignKey(nt => nt.SourceId);
            modelBuilder.Entity<NewsSourceNews>()
                .HasOne(ns => ns.NewsItem)
                .WithMany(n => n.Sources)
                .HasForeignKey(ns => ns.NewsItemId);

            modelBuilder.Entity<NewsTag>()
                .ToTable("news_tags")
                .HasIndex(t => t.Name);

            modelBuilder.Entity<NewsTagsNews>().ToTable("news_tags_news");

            modelBuilder.Entity<NewsTagsNews>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.TagNewsItems)
                .HasForeignKey(nt => nt.TagId);
            modelBuilder.Entity<NewsTagsNews>()
                .HasOne(nt => nt.NewsItem)
                .WithMany(t => t.Tags)
                .HasForeignKey(nt => nt.NewsItemId);

            modelBuilder.Entity<NewsSource>().ToTable("news_sources");
            modelBuilder.Entity<NewsSource>()
                .HasOne(s => s.Creator)
                .WithMany(u => u.CreatedNewsSources)
                .HasForeignKey(s => s.CreatorId);

            modelBuilder.Entity<Token>().ToTable("tokens");
        }
    }
}
