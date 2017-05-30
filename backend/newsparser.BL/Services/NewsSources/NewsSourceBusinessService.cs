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

        public IEnumerable<NewsSource> GetAllNewsSources(bool withUsers = false)
        {
            var newsSources = _newsSourceRepository.GetNewsSources().Include(n => n.UsersSources);
            return withUsers ?
                newsSources.Where(n => n.UsersSources.Any()) :
                newsSources;
        }

        public IEnumerable<NewsSource> GetAllNewsSourcesForUser(int userId)
        {
            return _newsSourceRepository.GetNewsSources()
                .Include(s => s.UsersSources)
                .Where(s =>  !s.UsersSources.Any() || 
                    s.UsersSources.Any(us => !us.IsPrivate || us.UserId == userId));
        }

        public IEnumerable<NewsSource> GetAvailableNewsSources(int userId)
        {
            return _newsSourceRepository.GetNewsSources()
                    .Include(s => s.UsersSources)
                    .Where(s => !s.UsersSources.Any() 
                        || (s.UsersSources.All(us => us.UserId != userId)
                        && s.UsersSources.Any(us => !us.IsPrivate)));
        }

        public IEnumerable<NewsSource> GetNewsSourcesPage(
            int userId, 
            out int total, 
            int pageIndex = 0, 
            int pageSize = 5, 
            string search = null, 
            bool subscribed = false)
        {
            var newsSources = subscribed ?
                GetNewsSourcesByUser(userId) :
                GetAllNewsSourcesForUser(userId);

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

            var existingNewsSource = _newsSourceRepository.GetNewsSourceByUrl(newsSource.FeedUrl);
            if (existingNewsSource != null)
            {
                throw new BusinessLayerException($"News source with RSS url {newsSource.FeedUrl} already exists");
            }

            try
            {
                return _newsSourceRepository.AddNewsSource(newsSource);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed inserting a news source with RSS url {newsSource.FeedUrl}", e);
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

        public NewsSource GetNewsSourceByUrl(string rssUrl)
        {
            return _newsSourceRepository.GetNewsSourceByUrl(rssUrl);
        }

        public IEnumerable<NewsSource> GetNewsSourcesByUser(int userId)
        {
            return _newsSourceRepository.GetNewsSourcesByUser(userId).Include(n => n.UsersSources);
        }

        public bool IsUserSubscribed(int sourceId, int userId)
        {
            return _newsSourceRepository.GetNewsSourcesByUser(userId).Any(s => s.Id == sourceId);
        }

        public void SubscribeUser(int sourceId, int userId, bool isPrivate = false)
        {
            if (_userRepository.GetUserById(userId) == null)
            {
                throw new BusinessLayerException("User does not exist");
            }

            var newsSource = GetNewsSourceById(sourceId);
            if (newsSource == null)
            {
                throw new BusinessLayerException("News source does not exist");
            }

            try
            {
                _newsSourceRepository.AddNewsSourceToUser(new UserNewsSource {
                    SourceId = sourceId,
                    UserId = userId,
                    IsPrivate = isPrivate
                });
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed adding news source {sourceId} to user {userId}", e);
            }
        }

        public void DeleteUserNewsSource(int sourceId, int userId)
        {
            try
            {
                _newsSourceRepository.DeleteUserNewsSource(sourceId, userId);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed deleting news source {sourceId} from user {userId}", e);
            }
        }

        public void DeleteNewsSource(NewsSource source)
        {
            if (source == null)
            {
                throw new BusinessLayerException($"News source cannot be null");
            }

            try
            {
                _newsSourceRepository.DeleteNewsSource(source);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed to delete news source with id {source.Id}", e);
            }
        }

        public bool IsSourcePrivateToUser(int sourceId, int userId)
        {
            return _newsSourceRepository.GetUsersPrivateNewsSources(userId).Any(s => s.Id == sourceId);
        }

        public bool CanDeletePrivateSource(int sourceId, int userId)
        {
            return IsSourcePrivateToUser(sourceId, userId) 
                && _newsSourceRepository.GetSourceUsers(sourceId).Count() == 1;
        }

        public bool IsSourceVisibleToUser(int sourceId, int userId)
        {
            return _newsSourceRepository.IsSourceVisibleToUser(sourceId, userId);
        }

        public void UnsubscribeUser(int sourceId, int userId)
        {
            var source = GetNewsSourceById(sourceId);
            if(!IsSourceVisibleToUser(source.Id, userId))
            {
                throw new EntityNotFoundException("News source was not found");
            }
            
            // If this source is private to some users, we first need to check
            // if the current user is the only one left subscribed. If it's true,
            // we can safely remove the source, if not - just unsubscribe the current user.
            if(CanDeletePrivateSource(sourceId, userId))
            {
                DeleteNewsSource(source);
            }
            else
            {
                DeleteUserNewsSource(sourceId, userId);
            }
        }
    }
}
