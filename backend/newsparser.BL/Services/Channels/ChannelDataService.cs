using System;
using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;
using NewsParser.DAL.Repositories.Channels;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.Repositories.Users;
using NewsParser.BL.Exceptions;

namespace NewsParser.BL.Services.Channels
{
    public class ChannelDataService: IChannelDataService
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IUserRepository _userRepository;

        public ChannelDataService(IChannelRepository channelRepository, IUserRepository userRepository)
        {
            _channelRepository = channelRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<Channel> GetAll(bool withUsers = false)
        {
            var newsSources = _channelRepository.GetAll().Include(n => n.Users);
            return withUsers ?
                newsSources.Where(n => n.Users.Any()) :
                newsSources;
        }

        public IEnumerable<Channel> GetForUpdate()
        {
            return _channelRepository
                .GetByUpdateDate()
                .Include(s => s.Users)
                .Where(s => s.Users.Any())
                .OrderByDescending(s => s.Users.Count());
        }

        public IEnumerable<Channel> GetAllForUser(int userId)
        {
            return _channelRepository.GetAll()
                .Include(s => s.Users)
                .Where(s =>  !s.Users.Any() || 
                    s.Users.Any(us => !us.IsPrivate || us.UserId == userId));
        }

        public IEnumerable<Channel> GetPage(
            int userId, 
            out int total, 
            int pageIndex = 0, 
            int pageSize = 5, 
            string search = null, 
            bool subscribed = false)
        {
            var newsSources = subscribed ?
                GetByUser(userId) :
                GetAllForUser(userId);

            if (!string.IsNullOrEmpty(search))
            {
                newsSources = newsSources.Where(s => s.Name.ToLower().Contains(search.ToLower()));
            }

            total = newsSources.Count();
            return newsSources
                .OrderByDescending(s => s.Users.Count())
                .ThenBy(s => s.Name)
                .Skip(pageIndex)
                .Take(pageSize);
        }

        public Channel GetById(int id)
        {
            return _channelRepository.GetById(id) ??
                throw new EntityNotFoundException("Channel was not found");
        }

        public Channel Add(Channel channel)
        {
            if (channel == null)
            {
                throw new ArgumentNullException(nameof(channel), $"Channel cannot be null");
            }

            var existingNewsSource = _channelRepository.GetByUrl(channel.FeedUrl);
            if (existingNewsSource != null)
            {
                throw new BusinessLayerException($"Channel with RSS url {channel.FeedUrl} already exists");
            }

            try
            {
                return _channelRepository.Add(channel);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed inserting a channel with the feed url {channel.FeedUrl}", e);
            }
        }

        public void Update(Channel newsSource)
        {
            if (newsSource == null)
            {
                throw new ArgumentNullException(nameof(newsSource), "Channel cannot be null");    
            }

            try
            {
                _channelRepository.Update(newsSource);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed to update channel with id {newsSource.Id}", e);
            }
        }

        public Channel GetByUrl(string rssUrl)
        {
            return _channelRepository.GetByUrl(rssUrl);
        }

        public IEnumerable<Channel> GetByUser(int userId)
        {
            return _channelRepository.GetByUser(userId).Include(n => n.Users);
        }

        public bool IsUserSubscribed(int sourceId, int userId)
        {
            return _channelRepository.GetByUser(userId).Any(s => s.Id == sourceId);
        }

        public void SubscribeUser(int sourceId, int userId, bool isPrivate = false)
        {
            if (_userRepository.GetUserById(userId) == null)
            {
                throw new BusinessLayerException("User does not exist");
            }

            var newsSource = GetById(sourceId);
            if (newsSource == null)
            {
                throw new BusinessLayerException("Channel does not exist");
            }

            try
            {
                _channelRepository.AddUser(new UserChannel {
                    ChannelId = sourceId,
                    UserId = userId,
                    IsPrivate = isPrivate
                });
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed adding channel {sourceId} to user {userId}", e);
            }
        }

        public void DeleteUserChannel(int sourceId, int userId)
        {
            try
            {
                _channelRepository.DeleteUserChannel(sourceId, userId);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed deleting channel {sourceId} from user {userId}", e);
            }
        }

        public void Delete(Channel source)
        {
            if (source == null)
            {
                throw new BusinessLayerException($"Channel cannot be null");
            }

            try
            {
                _channelRepository.Delete(source);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed to delete channel with id {source.Id}", e);
            }
        }

        public bool IsPrivateToUser(int sourceId, int userId)
        {
            return _channelRepository.GetPrivateByUser(userId).Any(s => s.Id == sourceId);
        }

        public bool CanDelete(int sourceId, int userId)
        {
            return IsPrivateToUser(sourceId, userId) 
                && _channelRepository.GetUsers(sourceId).Count() == 1;
        }

        public bool IsVisibleToUser(int sourceId, int userId)
        {
            return _channelRepository.IsVisibleToUser(sourceId, userId);
        }

        public void UnsubscribeUser(int sourceId, int userId)
        {
            var source = GetById(sourceId);
            if(!IsVisibleToUser(source.Id, userId))
            {
                throw new EntityNotFoundException("Channel was not found");
            }
            
            // If this source is private to some users, we first need to check
            // if the current user is the only one left subscribed. If it's true,
            // we can safely remove the source, if not - just unsubscribe the current user.
            if(CanDelete(sourceId, userId))
            {
                Delete(source);
            }
            else
            {
                DeleteUserChannel(sourceId, userId);
            }
        }
    }
}
