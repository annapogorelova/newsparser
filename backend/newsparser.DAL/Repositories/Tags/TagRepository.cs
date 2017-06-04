using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Tags
{
    /// <summary>
    /// Provides a functionality to access the NewsTag entity data
    /// </summary>
    public class TagRepository: ITagRepository
    {
        private readonly AppDbContext _dbContext;

        public TagRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Tag> GetAll()
        {
            return _dbContext.Tags;
        }

        public Tag GetById(int id)
        {
            return _dbContext.Tags.Find(id);
        }

        public Tag GetByName(string name)
        {
            return _dbContext.Tags.FirstOrDefault(t => t.Name == name.ToLowerInvariant());
        }

        public Tag Add(Tag newsTag)
        {
            if (newsTag == null)
            {
                throw new ArgumentNullException(nameof(newsTag), "News tag cannot be null");
            }

            _dbContext.Tags.Add(newsTag);
            _dbContext.SaveChanges();
            return _dbContext.Entry(newsTag).Entity;
        }

        public void Add(IEnumerable<Tag> newsTags)
        {
            if (newsTags == null)
            {
                throw new ArgumentNullException(nameof(newsTags), "News tags collection cannot be null");
            }

            _dbContext.Tags.AddRange(newsTags);
            _dbContext.SaveChanges();
        }

        public IQueryable<Tag> GetByFeedItemId(int newsItemId)
        {
            return _dbContext.TagFeedItems.Include(nt => nt.Tag)
                .Where(nt => nt.FeedItemId == newsItemId).Select(nt => nt.Tag);
        }
    }
}
