﻿using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Repositories.News
{
    /// <summary>
    /// Provides a functionality to access the NewsItem entity data
    /// </summary>
    public interface INewsRepository
    {
        /// <summary>
        /// Get all news items
        /// </summary>
        /// <returns>IQueryable of NewsItem</returns>
        IQueryable<NewsItem> GetNews();

        /// <summary>
        /// Get news items by source id
        /// </summary>
        /// <param name="sourceId">Source id</param>
        /// <returns>IQueryable of NewsItem</returns>
        IQueryable<NewsItem> GetNewsBySource(int sourceId);

        /// <summary>
        /// Gets new items by tag name
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns>IQueryable of NewsItem</returns>
        IQueryable<NewsItem> GetNewsByTagName(string tagName);

        /// <summary>
        /// Gets news items by tag id
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <returns>IQueryable of NewsItem</returns>
        IQueryable<NewsItem> GetNewsByTagId(int tagId);

        /// <summary>
        /// Gets news item by id
        /// </summary>
        /// <param name="id">News item id</param>
        /// <returns>NewsItem object</returns>
        NewsItem GetNewsById(int id);

        /// <summary>
        /// Gets news item by the link to it's source
        /// </summary>
        /// <param name="linkToSource">Link to source string</param>
        /// <returns>NewsItem object</returns>
        NewsItem GetNewsItemByLink(string linkToSource);

        /// <summary>
        /// Inserts a news item
        /// </summary>
        /// <param name="newsItem">NewsItem object</param>
        /// <returns>Inserted NewsItem object</returns>
        NewsItem AddNewsItem(NewsItem newsItem);

        /// <summary>
        /// Inserts a list of news items
        /// </summary>
        /// <param name="newsItems">IEnumerable of NewsItem</param>
        void AddNewsItems(IEnumerable<NewsItem> newsItems);

        /// <summary>
        /// Inserts a NewsTagNews record that connects news item and tag
        /// </summary>
        /// <param name="newsItemId">News item id</param>
        /// <param name="tagId">Tag id</param>
        void AddNewsItemTag(int newsItemId, int tagId);

        /// <summary>
        /// Inserts a UserNews record that connects user and news item
        /// </summary>
        /// <param name="newsItemId">News item id</param>
        /// <param name="userId">User id</param>
        void AddNewsItemToUser(int newsItemId, int userId);

        /// <summary>
        /// Inserts a range of UserNews items that connect a user to a range of news
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="news">IEnumerable of NewsItem</param>
        void AddNewsToUser(int userId, IEnumerable<NewsItem> news);

        /// <summary>
        /// Deletes a news item
        /// </summary>
        /// <param name="newsItem">NewsItem object</param>
        void DeleteNewsItem(NewsItem newsItem);

        /// <summary>
        /// Deletes a range of news
        /// </summary>
        void DeleteNews(IEnumerable<NewsItem> news);
    }
}
