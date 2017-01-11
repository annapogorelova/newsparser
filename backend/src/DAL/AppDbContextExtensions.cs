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
            // Inserting data
            if (!dbContext.NewsCategories.Any())
            {
                AddNewsCategories(dbContext);
            }

            if (!dbContext.NewsTags.Any())
            {
                AddNewsTags(dbContext);
            }

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

        private static void AddNewsCategories(AppDbContext dbContext)
        {
            dbContext.NewsCategories.AddRange(new List<NewsCategory>()
            {
                new NewsCategory()
                {
                    Name = "Politics"
                },
                new NewsCategory()
                {
                    Name = "Sports"
                },
                new NewsCategory()
                {
                    Name = "Technology"
                },
                new NewsCategory()
                {
                    Name = "Science"
                },
                new NewsCategory()
                {
                    Name = "Art"
                },
                new NewsCategory()
                {
                    Name = "Crime"
                },
                new NewsCategory()
                {
                    Name = "Gossip"
                }
            });
        }

        private static void AddNewsTags(AppDbContext dbContext)
        {
            dbContext.NewsTags.AddRange(new List<NewsTag>()
            {
                new NewsTag()
                {
                    Name = "lviv"
                },
                new NewsTag()
                {
                    Name = "celebration"
                },
                new NewsTag()
                {
                    Name = "computer science"
                },
                new NewsTag()
                {
                    Name = "event"
                }
            });
        }

        private static void AddNewsSources(AppDbContext dbContext)
        {
            dbContext.NewsSources.AddRange(new List<NewsSource>()
            {
                new NewsSource()
                {
                    DateAdded = DateTime.UtcNow,
                    MainUrl = "https://zik.ua",
                    Name = "ZIK"
                },
                new NewsSource()
                {
                    DateAdded = DateTime.UtcNow,
                    MainUrl = "https://bbc.com",
                    Name = "BBC"
                },
                new NewsSource()
                {
                    DateAdded = DateTime.UtcNow,
                    MainUrl = "https://cnn.com",
                    Name = "CNN"
                }
            });
        }

        private static void AddNews(AppDbContext dbContext)
        {
            var sources = dbContext.NewsSources.Local;
            var categories = dbContext.NewsCategories.Local;

            dbContext.News.AddRange(new List<NewsItem> ()
            {
                new NewsItem()
                {
                    Title = "IRON MAN in Ukraine",
                    Description = "Annual Iron Man Triathlon contest is going to be held in Lviv this year",
                    DateAdded = DateTime.UtcNow,
                    LinkToSource = "https://zik.ua/news/2383289",
                    SourceId = sources.FirstOrDefault(s => s.Name == "ZIK") != null ? 
                        sources.First(s => s.Name == "ZIK").Id : sources.First().Id,
                    CategoryId = categories.FirstOrDefault(c => c.Name == "Sports") != null ?
                        categories.First(c => c.Name == "Sports").Id : categories.First().Id
                },
                new NewsItem()
                {
                    Title = "Woman got killed by her husband",
                    Description = "Woman, 37 years old got killed this thursday by her new husband Bill",
                    DateAdded = DateTime.UtcNow,
                    LinkToSource = "https://cnn.com/news/380nf00f",
                    SourceId = sources.FirstOrDefault(s => s.Name == "CNN") != null ?
                        sources.First(s => s.Name == "CNN").Id : sources.First().Id,
                    CategoryId = categories.FirstOrDefault(c => c.Name == "Crime") != null ?
                        categories.First(c => c.Name == "Crime").Id : categories.First().Id
                },
                new NewsItem()
                {
                    Title = "Are trees what we think they are?",
                    Description = "Trees occured to have a primitive form of intellect",
                    DateAdded = DateTime.UtcNow,
                    LinkToSource = "https://bbc.com/news/380nf00f",
                    SourceId = sources.FirstOrDefault(s => s.Name == "BBC") != null ?
                        sources.First(s => s.Name == "BBC").Id : sources.First().Id,
                    CategoryId = categories.FirstOrDefault(c => c.Name == "Science") != null ?
                        categories.First(c => c.Name == "Science").Id : categories.First().Id
                }
            });
        }

        private static void AddUsers(AppDbContext dbContext)
        {
            dbContext.Add(new User()
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
