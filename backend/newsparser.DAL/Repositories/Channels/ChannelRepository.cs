using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Repositories.Channels
{
    /// <summary>
    /// Provides functionality for accessing the channels data
    /// </summary>
    public class ChannelRepository: IChannelRepository
    {
        private readonly AppDbContext _dbContext;

        public ChannelRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Channel> GetAll()
        {
            return _dbContext.Channels;
        }

        public IQueryable<Channel> GetByUpdateDate()
        {
            return _dbContext.Channels
                .FromSql(@"SELECT * FROM channels 
                    WHERE DATE_ADD(DateFeedUpdated, INTERVAL UpdateIntervalMinutes MINUTE) <= NOW()");
        }

        public IQueryable<Channel> GetByUser(int userId)
        {
            return _dbContext.Channels
                .Include(s => s.Users)
                .Where(n => n.Users.Any(us => us.UserId == userId));
        }
 
        public Channel GetById(int id)
        {
            return _dbContext.Channels
                .Include(s => s.Users)
                .FirstOrDefault(s => s.Id == id);
        }

        public Channel GetByUrl(string rssUrl)
        {
            return _dbContext.Channels
                .Include(s => s.Users)
                .FirstOrDefault(n => n.FeedUrl == rssUrl);
        }

        public Channel Add(Channel newsSource)
        {
            if (newsSource == null)
            {
                throw new ArgumentNullException(nameof(newsSource), "Channel cannot be null");
            }

            _dbContext.Channels.Add(newsSource);
            _dbContext.SaveChanges();
            return _dbContext.Entry(newsSource).Entity;
        }

        public void Update(Channel newsSource)
        {
            if (newsSource == null)
            {
                throw new ArgumentNullException(nameof(newsSource), "Channel cannot be null");
            }

            _dbContext.Entry(newsSource).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void Delete(Channel newsSource)
        {
            if (newsSource == null)
            {
                throw new ArgumentNullException(nameof(newsSource), "Channel cannot be null");
            }

            _dbContext.Channels.Remove(newsSource);
            _dbContext.SaveChanges();
        }

        public void AddUser(UserChannel userChannel)
        {
            _dbContext.UserChannels.Add(userChannel);
            _dbContext.SaveChanges();
        }

        public void DeleteUserChannel(int sourceId, int userId)
        {
            var userChannel =
                _dbContext.UserChannels.FirstOrDefault(us => us.ChannelId == sourceId && us.UserId == userId);
            if (userChannel == null)
            {
                throw new NullReferenceException($"UserChannel with user id {userId} and channel id {sourceId} does not exist");
            }

            _dbContext.UserChannels.Remove(userChannel);
            _dbContext.SaveChanges();
        }

        public IQueryable<UserChannel> GetUsers(int sourceId)
        {
            return _dbContext.UserChannels.Where(s => s.ChannelId == sourceId);
        }

        public bool IsVisibleToUser(int sourceId, int userId)
        {
            return !_dbContext.UserChannels.Any(us => us.ChannelId == sourceId) ||
                !_dbContext.UserChannels.Where(us => us.ChannelId == sourceId)
                    .All(us => us.UserId != userId  && us.IsPrivate);
        }

        public IQueryable<Channel> GetPrivateByUser(int userId)
        {
            return _dbContext.Channels
                .Include(s => s.Users)
                .Where(s => s.Users.Any(us => us.UserId == userId && us.IsPrivate));
        }
    }
}
