using System;
using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;
using NewsParser.DAL.NewsTags;
using NewsParser.BL.Exceptions;

namespace NewsParser.BL.Services.NewsTags
{
    public class NewsTagBusinessService: INewsTagBusinessService
    {
        private readonly INewsTagRepository _newsTagRepository;

        public NewsTagBusinessService(INewsTagRepository newsTagRepository)
        {
            _newsTagRepository = newsTagRepository;
        }

        public IEnumerable<NewsTag> GetNewsTags()
        {
            return _newsTagRepository.GetNewsTags();
        }

        public NewsTag GetNewsTagById(int id)
        {
            return _newsTagRepository.GetNewsTagById(id) ??
                throw new EntityNotFoundException("News tag was not found");
        }

        public NewsTag GetNewsTagByName(string name)
        {
            return _newsTagRepository.GetNewsTagByName(name) ??
                throw new EntityNotFoundException("News tag was not found");
        }

        public NewsTag AddNewsTag(NewsTag newsTag)
        {
            if (newsTag == null)
            {
                throw new ArgumentNullException(nameof(newsTag), "newsTag cannot be null");
            }

            var existingTag = _newsTagRepository.GetNewsTagByName(newsTag.Name);

            if (existingTag != null)
            {
                throw new BusinessLayerException($"Tag {newsTag.Name} already exists");
            }

            try
            {
                return _newsTagRepository.AddNewsTag(newsTag);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed inserting the news tag {newsTag.Name}", e);
            }
        }

        public void AddNewsTags(List<NewsTag> newsTags)
        {
            if (newsTags == null || !newsTags.Any())
            {
                return;
            }

            try
            {
                _newsTagRepository.AddNewsTags(newsTags);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed inserting a list of news tags", e);
            }
        }
    }
}
