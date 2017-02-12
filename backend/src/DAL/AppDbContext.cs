using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.Models;

namespace NewsParser.DAL
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<NewsItem> News { get; set; }
        public DbSet<NewsSource> NewsSources { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }
        public DbSet<NewsTagsNews> NewsTagsNews { get; set; }
        public DbSet<UserNews> UserNews { get; set; }
        public DbSet<UserNewsSource> UserSources { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserNews>().ToTable("UserNews");

            modelBuilder.Entity<UserNews>()
                .HasOne(un => un.User)
                .WithMany(u => u.News)
                .HasForeignKey(un => un.UserId);

            modelBuilder.Entity<UserNews>()
                .HasOne(un => un.NewsItem)
                .WithMany(n => n.NewsItemUsers)
                .HasForeignKey(un => un.NewsItemId);

            modelBuilder.Entity<UserNewsSource>()
                .HasOne(ns => ns.User)
                .WithMany(u => u.NewsSources)
                .HasForeignKey(ns => ns.UserId);

            modelBuilder.Entity<UserNewsSource>()
                .HasOne(ns => ns.NewsSource)
                .WithMany(u => u.Users)
                .HasForeignKey(ns => ns.SourceId);

            modelBuilder.Entity<NewsItem>().ToTable("News");

            modelBuilder.Entity<NewsItem>()
                .HasOne(n => n.Source)
                .WithMany(s => s.News)
                .HasForeignKey(n => n.SourceId);

            modelBuilder.Entity<NewsTag>().ToTable("NewsTags");

            modelBuilder.Entity<NewsTagsNews>().ToTable("NewsTagsNews");

            modelBuilder.Entity<NewsTagsNews>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.TagNewsItems)
                .HasForeignKey(nt => nt.TagId);
            modelBuilder.Entity<NewsTagsNews>()
                .HasOne(nt => nt.NewsItem)
                .WithMany(t => t.NewsItemTags)
                .HasForeignKey(nt => nt.NewsItemId);

            modelBuilder.Entity<NewsSource>().ToTable("NewsSources");
        }
    }
}
