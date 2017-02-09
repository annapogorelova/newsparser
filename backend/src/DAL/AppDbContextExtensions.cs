using System;
using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.DAL
{
    public static class AppDbContextExtensions
    {
        public static void EnsureSeedData(this AppDbContext dbContext)
        {
            if (!dbContext.NewsSources.Any())
            {
                AddNewsSources(dbContext);
            }

            if (!dbContext.News.Any())
            {
                AddNews(dbContext);
            }

            if (!dbContext.Users.Any())
            {
                AddUsers(dbContext);
            }

            if (dbContext.ChangeTracker.HasChanges())
            {
                dbContext.SaveChanges();
            }
        }

        private static void AddNewsSources(AppDbContext dbContext)
        {
            dbContext.NewsSources.AddRange(new List<NewsSource>()
            {
                new NewsSource
                {
                    RssUrl = "https://habrahabr.ru/rss/all/",
                    Name = "Habrahabr"
                },
                new NewsSource
                {
                    RssUrl = "https://geektimes.ru/rss/all/",
                    Name = "Geektimes"
                },
                new NewsSource
                {
                    RssUrl = "https://css-tricks.com/feed/",
                    Name = "CSS Tricks"
                },
                new NewsSource
                {
                    RssUrl = "http://arzamas.academy/feed_v1.rss",
                    Name = "Arzamas"
                }
            });
        }

        private static void AddNews(AppDbContext dbContext)
        {
            var sources = dbContext.NewsSources.Local.Count != 0 ? 
                dbContext.NewsSources.Local.ToList() : dbContext.NewsSources.ToList();

            dbContext.News.AddRange(new List<NewsItem> ()
            {
                new NewsItem
                {
                    Title = "Welcome!",
                    Description = "Welcome to the News Parser",
                    DateAdded = DateTime.UtcNow,
                    LinkToSource = "https://localhost:50451/news",
                    SourceId = sources.First().Id
                }
            });
        }

        private static void AddUsers(AppDbContext dbContext)
        {
            dbContext.Add(new User
            {
                DateAdded = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                Password = "AQAAAAEAACcQAAAAEIuIbDmD0yrdqLtSGiiWobFEBFK7bdlrJEPnornIOA+RGlTB8yoYwDOX/Cbr2viTlw==", //hash for "tolochko" word
                Email = "anya.pogorelova@gmail.com",
                FirstName = "Anna",
                LastName = "Pohorielova"
            });
        }
    }
}
