using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.Models;

namespace NewsParser.DAL
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<NewsItem> News { get; set; }
        public DbSet<NewsSource> NewsSources { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<NewsItem>().ToTable("News");
            modelBuilder.Entity<NewsItem>()
                .HasOne(n => n.Category)
                .WithMany(c => c.NewsItems)
                .HasForeignKey(n => n.CategoryId);
            modelBuilder.Entity<NewsItem>()
                .HasOne(n => n.Source)
                .WithMany(s => s.News)
                .HasForeignKey(n => n.SourceId);

            modelBuilder.Entity<NewsTag>().ToTable("Tags");

            modelBuilder.Entity<NewsTagsNews>().ToTable("NewsTagsNews");
            modelBuilder.Entity<NewsTagsNews>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NewsTags)
                .HasForeignKey(nt => nt.TagId);
            modelBuilder.Entity<NewsTagsNews>()
                .HasOne(nt => nt.NewsItem)
                .WithMany(t => t.Tags)
                .HasForeignKey(nt => nt.NewsItemId);

            modelBuilder.Entity<NewsSource>().ToTable("NewsSources");
            modelBuilder.Entity<NewsCategory>().ToTable("NewsCategories");
        }
    }
}
