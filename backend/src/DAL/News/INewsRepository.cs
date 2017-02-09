using System;
using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.News
{
    public interface INewsRepository
    {
        IQueryable<NewsItem> GetNews(int startIndex = 0, int numResults = 5, int? sourceId = null);
        IQueryable<NewsItem> GetNewsBySource(int sourceId);
        IQueryable<NewsItem> GetNewsByTagName(string tagName);
        IQueryable<NewsItem> GetNewsByTagId(int tagId);

        NewsItem GetNewsById(int id);
        NewsItem GetNewsItemByLink(string linkToSource);

        NewsItem AddNewsItem(NewsItem newsItem);
        void AddNewsItems(IEnumerable<NewsItem> newsItems);
        void AddTagToNewsItem(NewsItem newsItem, NewsTag tag);

        void DeleteNewsItem(int id);
        void DeleteNews(DateTime dateTo, DateTime? dateFrom = null);
    }
}
