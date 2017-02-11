﻿using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.NewsTags
{
    /// <summary>
    /// Repository for accessing the news tags data
    /// </summary>
    public class NewsTagRepository: INewsTagRepository
    {
        private readonly AppDbContext _dbContext;

        public NewsTagRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all news tags
        /// </summary>
        /// <returns>IQueryable of NewsTag</returns>
        public IQueryable<NewsTag> GetNewsTags()
        {
            return _dbContext.NewsTags;
        }

        /// <summary>
        /// Gets news tag by id
        /// </summary>
        /// <param name="id">News tag's id</param>
        /// <returns>NewsTag object</returns>
        public NewsTag GetNewsTagById(int id)
        {
            return _dbContext.NewsTags.Find(id);
        }

        /// <summary>
        /// Gets news tag by name
        /// </summary>
        /// <param name="name">News tag's name</param>
        /// <returns>NewsTag object</returns>
        public NewsTag GetNewsTagByName(string name)
        {
            return _dbContext.NewsTags.FirstOrDefault(t => t.Name == name);
        }

        /// <summary>
        /// Inserts a news tag
        /// </summary>
        /// <param name="newsTag">NewsTag object</param>
        /// <returns>NewsTag object</returns>
        public NewsTag AddNewsTag(NewsTag newsTag)
        {
            _dbContext.NewsTags.Add(newsTag);
            _dbContext.SaveChanges();
            return _dbContext.Entry(newsTag).Entity;
        }

        /// <summary>
        /// Inserts a list of news tags
        /// </summary>
        /// <param name="newsTags">List of NewsTag</param>
        public void AddNewsTags(IEnumerable<NewsTag> newsTags)
        {
            _dbContext.NewsTags.AddRange(newsTags);
            _dbContext.SaveChanges();
        }
    }
}