using System;
using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;
using NewsParser.DAL.Repositories.NewsSources;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.Repositories.Users;
using NewsParser.BL.Exceptions;

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

        public IEnumerable<NewsSource> GetNewsSources(bool hasUsers = false)
        {
            var newsSources = _newsSourceRepository.GetNewsSources().Include(n => n.Users);
            return hasUsers ?
                newsSources.Where(n => n.Users.Any()) :
                newsSources;
        }

        public IEnumerable<NewsSource> GetAvailableNewsSources(int userId)
        {
            return _newsSourceRepository.GetNewsSources()
                    .Include(s => s.Users)
                    .Where(s => s.Users.All(u => u.UserId != userId));
        }

        public IEnumerable<NewsSource> GetNewsSourcesPage(out int total, int pageIndex = 0, int pageSize = 5, string search = null,
            bool subscribed = false, int? userId = null)
        {
            var newsSources = subscribed && userId.HasValue ?
                GetNewsSourcesByUser(userId.Value) :
                GetNewsSources();

            if (!string.IsNullOrEmpty(search))
            {
                newsSources = newsSources.Where(s => s.Name.ToLower().Contains(search.ToLower()));
            }

            total = newsSources.Count();
            return newsSources.OrderBy(s => s.Name).Skip(pageIndex).Take(pageSize);
        }

        public NewsSource GetNewsSourceById(int id)
        {
            return _newsSourceRepository.GetNewsSourceById(id) ??
                throw new EntityNotFoundException("News source was not found");
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
                throw new BusinessLayerException($"News source with RSS url {newsSource.RssUrl} already exists");
            }

            try
            {
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

        public void DeleteUserNewsSource(int sourceId, int userId)
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
                _newsSourceRepository.DeleteUserNewsSource(sourceId, userId);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed deleting news source {sourceId} from user {userId}", e);
            }
        }

        public IEnumerable<NewsSource> GetNewsSourcesByUser(int userId)
        {
            return _newsSourceRepository.GetNewsSourcesByUser(userId).Include(n => n.Users);
        }

        public bool IsUserSubscribed(int sourceId, int userId)
        {
            return _newsSourceRepository.GetNewsSourcesByUser(userId).Any(s => s.Id == sourceId);
        }
    }
}
