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

            if (!dbContext.Users.Any())
            {
                AddUsers(dbContext);
            }

            if (!dbContext.UserSources.Any())
            {
                AddNewsSourcesToUsers(dbContext);
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
                    Name = "Хабрахабр / Все публикации",
                    Description = "Все публикации на Хабрахабре",
                    ImageUrl = "https://habrahabr.ru/images/logo.png",
                    WebsiteUrl = "https://habrahabr.ru/"
                },
                new NewsSource
                {
                    RssUrl = "https://geektimes.ru/rss/all/",
                    Name = "Geektimes / Все публикации",
                    Description = "Все публикации на Geektimes",
                    ImageUrl = "https://geektimes.ru/images/logo.png",
                    WebsiteUrl = "https://geektimes.ru/"
                },
                new NewsSource
                {
                    RssUrl = "https://css-tricks.com/feed/",
                    Name = "CSS-Tricks",
                    Description = "Tips, Tricks, and Techniques on using Cascading Style Sheets.",
                    WebsiteUrl = "https://css-tricks.com/"
                },
                new NewsSource
                {
                    RssUrl = "http://arzamas.academy/feed_v1.rss",
                    Name = "Arzamas | Всё",
                    Description = "Новые курсы каждый четверг и дополнительные материалы в течение недели",
                    ImageUrl = "http://arzamas.academy/apple-touch-icon.png",
                    WebsiteUrl = "http://arzamas.academy/"
                },
                new NewsSource
                {
                    RssUrl = "https://habrahabr.ru/rss/flows/develop/all/",
                    Name = "Хабрахабр / Все публикации в потоке Разработка",
                    Description = "Все публикации в потоке Разработка на Хабрахабре",
                    ImageUrl = "https://habrahabr.ru/images/logo.png",
                    WebsiteUrl = "https://habrahabr.ru/"
                },
                new NewsSource
                {
                    RssUrl = "https://habrahabr.ru/rss/hub/programming/",
                    Name = "Хабрахабр / Программирование / Интересные публикации",
                    Description = "Интересные публикации из хаба «Программирование» на Хабрахабре",
                    ImageUrl = "https://habrahabr.ru/images/logo.png",
                    WebsiteUrl = "https://habrahabr.ru/"
                },
                new NewsSource
                {
                    RssUrl = "https://postnauka.ru/feed",
                    Name = "ПостНаука",
                    Description = "все, что вы хотели знать о науке, но не знали, у кого спросить",
                    WebsiteUrl = "https://postnauka.ru/"
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
                EmailConfirmed = true
            });
        }

        private static void AddNewsSourcesToUsers(AppDbContext dbContext)
        {
            var newsSources = dbContext.NewsSources.Local.Any() ? 
                dbContext.NewsSources.Local.ToList() : dbContext.NewsSources.ToList();
            var user = dbContext.Users.Local.Any() ? dbContext.Users.Local.First() : dbContext.Users.First();

            foreach (var newsSource in newsSources)
            {
                dbContext.UserSources.Add(new UserNewsSource()
                {
                    SourceId = newsSource.Id,
                    UserId = user.Id
                });
            }

            dbContext.SaveChanges();
        }
    }
}
