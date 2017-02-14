using System;
using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;
using NewsParser.DAL.Repositories.News;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.NewsTags;
using NewsParser.DAL.Repositories.Users;

namespace NewsParser.BL.News
{
    /// <summary>
    /// Implementation of INewsBusinessService using INewsRepository
    /// </summary>
    public class NewsBusinessService: INewsBusinessService
    {
        private readonly INewsRepository _newsRepository;
        private readonly INewsTagRepository _newsTagRepository;
        private readonly IUserRepository _userRepository;

        public NewsBusinessService(INewsRepository newsRepository, INewsTagRepository newsTagRepository, IUserRepository userRepository)
        {
            _newsRepository = newsRepository;
            _newsTagRepository = newsTagRepository;
            _userRepository = userRepository;
        }

        public IQueryable<NewsItem> GetNewsPage(int pageIndex = 0, int pageSize = 5, int? sourceId = null, int? userId = null)
        {
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "Page index must be greater than 0");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0");
            }

            var news = _newsRepository.GetNews();

            if (sourceId.HasValue)
            {
                news = news.Where(n => n.SourceId == sourceId.Value);
            }

            if (userId.HasValue)
            {
                news = news.Include(n => n.Source).ThenInclude(n => n.Users)
                    .Where(n => n.Source.Users.Any(u => u.UserId == userId.Value));
            }

            return news.OrderByDescending(n => n.DateAdded).Skip(pageIndex).Take(pageSize);
        }

        public IQueryable<NewsItem> GetNewsBySource(int sourceId)
        {
            return _newsRepository.GetNewsBySource(sourceId);
        }

        public NewsItem GetNewsItemById(int id)
        {
            return _newsRepository.GetNewsById(id);
        }

        public NewsItem GetNewsItemByLink(string linkToSource)
        {
            return _newsRepository.GetNewsItemByLink(linkToSource);
        }

        public NewsItem AddNewsItem(NewsItem newsItem)
        {
            if (_newsRepository.GetNewsItemByLink(newsItem.LinkToSource) != null)
            {
                return null;
            }

            try
            {
                return _newsRepository.AddNewsItem(newsItem);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException("Failed to add a news item", e);
            }
        }

        public void AddTagToNewsItem(int newsItemId, int tagId)
        {
            var newsItem = _newsRepository.GetNewsById(newsItemId);
            if (newsItem == null)
            {
                throw new ArgumentException($"News item with id {newsItemId} does not exist", nameof(newsItemId));
            }

            var tag = _newsTagRepository.GetNewsTagById(tagId);
            if (tag == null)
            {
                throw new ArgumentException($"News tag does with id {tagId} not exist", nameof(tagId));
            }

            try
            {
                _newsRepository.AddNewsItemTag(newsItemId, tagId);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed adding a tag with id {tagId} to news item with id {newsItemId}", e);
            }
        }

        public void AddTagsToNewsItem(int newsItemId, List<string> tags)
        {
            foreach (var tag in tags)
            {
                var newsTag = _newsTagRepository.GetNewsTagByName(tag) ??
                                      _newsTagRepository.AddNewsTag(new NewsTag { Name = tag });
                AddTagToNewsItem(newsItemId, newsTag.Id);
            }
        }

        public void DeleteNewsItem(int id)
        {
            var newsItem = _newsRepository.GetNewsById(id);
            if (newsItem == null)
            {
                throw new BusinessLayerException($"News item with id {id} does not exist");
            }

            try
            {
                _newsRepository.DeleteNewsItem(newsItem);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed to delete news item with id {id}", e);
            }
        }
    }
}
