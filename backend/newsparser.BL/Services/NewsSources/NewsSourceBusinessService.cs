﻿using System;
using System.Linq;
using NewsParser.DAL.Models;
using NewsParser.DAL.Repositories.NewsSources;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.Repositories.Users;

namespace NewsParser.BL.Services.NewsSources
{
    public class NewsSourceBusinessService: INewsSourceBusinessService
    {
        private readonly INewsSourceRepository _newsSourceRepository;
        private readonly IUserRepository _userRepository;

        public NewsSourceBusinessService(INewsSourceRepository newsSourceRepository, IUserRepository userRepository)
        {
            _newsSourceRepository = newsSourceRepository;
            _userRepository = userRepository;
        }

        public IQueryable<NewsSource> GetNewsSources(bool hasUsers = false)
        {
            var newsSources = _newsSourceRepository.GetNewsSources();
            return hasUsers ?
                newsSources.Include(n => n.Users).Where(n => n.Users.Any()) :
                newsSources;
        }

        public IQueryable<NewsSource> GetUserNewsSources(int userId)
        {
            return _newsSourceRepository.GetNewsSourcesByUser(userId);
        } 

        public NewsSource GetNewsSourceById(int id)
        {
            return _newsSourceRepository.GetNewsSourceById(id);
        }

        public NewsSource AddNewsSource(NewsSource newsSource)
        {
            if (newsSource == null)
            {
                throw new ArgumentNullException(nameof(newsSource), $"News source cannot be null");
            }

            var existingNewsSource = _newsSourceRepository.GetNewsSourceByUrl(newsSource.RssUrl);
            if (existingNewsSource != null)
            {
                throw new BusinessLayerException($"News source width RSS url {newsSource.RssUrl} already exists");
            }

            try
            {
                if (newsSource.Name.Length > 100)
                {
                    newsSource.Name = $"{newsSource.Name.Substring(0, 97)}...";
                }
                return _newsSourceRepository.AddNewsSource(newsSource);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed inserting a news source with RSS url {newsSource.RssUrl}", e);
            }
        }

        public void UpdateNewsSource(NewsSource newsSource)
        {
            if (newsSource == null)
            {
                throw new ArgumentNullException(nameof(newsSource), "News source cannot be null");    
            }

            if (_newsSourceRepository.GetNewsSourceById(newsSource.Id) == null)
            {
                throw new BusinessLayerException($"News source with id {newsSource.Id} does not exist");
            }

            try
            {
                _newsSourceRepository.UpdateNewsSource(newsSource);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed to update news source with id {newsSource.Id}", e);
            }
        }

        public void DeleteNewsSource(int id)
        {
            var newsSource = _newsSourceRepository.GetNewsSourceById(id);
            if (newsSource == null)
            {
                throw new BusinessLayerException($"News source with id {id} does not exist");
            }

            try
            {
                _newsSourceRepository.DeleteNewsSource(newsSource);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed to delete news source with id {newsSource.Id}", e);
            }
        }

        public NewsSource GetNewsSourceByUrl(string rssUrl)
        {
            return _newsSourceRepository.GetNewsSourceByUrl(rssUrl);
        }

        public void AddNewsSourceToUser(int sourceId, int userId)
        {
            if (_userRepository.GetUserById(userId) == null)
            {
                throw new BusinessLayerException($"User with id {userId} does not exist");
            }

            if (GetNewsSourceById(sourceId) == null)
            {
                throw new BusinessLayerException($"News source with id {sourceId} does not exist");
            }

            try
            {
                _newsSourceRepository.AddNewsSourceToUser(sourceId, userId);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed adding news source {sourceId} to user {userId}", e);
            }
        }
    }
}
