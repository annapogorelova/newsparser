using System;
using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;
using NewsParser.DAL.Repositories.News;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.NewsTags;
using NewsParser.BL.Exceptions;
using NewsParser.DAL.Repositories.NewsSources;

namespace NewsParser.BL.Services.News
{
    /// <summary>
    /// Implementation of INewsBusinessService using INewsRepository
    /// </summary>
    public class NewsBusinessService: INewsBusinessService
    {
        private readonly INewsRepository _newsRepository;
        private readonly INewsTagRepository _newsTagRepository;

        private readonly INewsSourceRepository _newsSourceRepository;

        public NewsBusinessService(
            INewsRepository newsRepository, 
            INewsTagRepository newsTagRepository,
            INewsSourceRepository newsSourceRepository)
        {
            _newsRepository = newsRepository;
            _newsTagRepository = newsTagRepository;
            _newsSourceRepository = newsSourceRepository;
        }

        public IEnumerable<NewsItem> GetNewsPage(
            int pageIndex = 0, 
            int pageSize = 5,
            int? userId = null, 
            string search = null,
            int[] sourcesIds = null, 
            string[] tags = null
        )
        {
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "Page index must be greater than 0");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0");
            }

            return _newsRepository.GetNewsPage(pageIndex, pageSize, userId, search, sourcesIds, tags);
        }

        public IEnumerable<NewsItem> GetNewsBySource(int sourceId)
        {
            return _newsRepository.GetNewsBySource(sourceId);
        }

        public NewsItem GetNewsItemById(int id)
        {
            return _newsRepository.GetNewsById(id) ?? 
                throw new EntityNotFoundException("News item was not found");
        }

        public NewsItem GetNewsItemByLink(string linkToSource)
        {
            return _newsRepository.GetNewsItemByLink(linkToSource) ??
                throw new EntityNotFoundException("News item was not found");
        }

        public NewsItem AddNewsItem(NewsItem newsItem)
        {
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
                var newsTag = _newsTagRepository.GetNewsTagByName(tag);
                if(newsTag == null)
                {
                    newsTag = _newsTagRepository.AddNewsTag(new NewsTag { Name = tag.ToLowerInvariant() });
                }

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

        public bool NewsItemExists(string guid)
        {
            return _newsRepository.GetNewsItemByGuid(guid) != null;
        }

        public void AddSourceToNewsItem(int newsItemId, int sourceId)
        {
            _newsRepository.AddNewsItemSource(newsItemId, sourceId);
        }

        public NewsItem GetNewsItemByGuid(string guid)
        {
            return _newsRepository.GetNewsItemByGuid(guid);
        }

        public void UpdateNewsItem(int newsItemId, int sourceId, List<string> tags = null)
        {
            try
            {
                if(!_newsRepository.NewsItemHasSource(newsItemId, sourceId))
                {
                    AddSourceToNewsItem(newsItemId, sourceId);
                }

                var newsItemTags = _newsTagRepository.GetNewsTagsByNewsItemId(newsItemId).ToList();
                var newTags = tags.Except(newsItemTags.Select(t => t.Name)).ToList();

                if(newTags.Any())
                {
                    AddTagsToNewsItem(newsItemId, newTags);
                }
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed to update news item with id {newsItemId}", e);
            }
        }
    }
}
