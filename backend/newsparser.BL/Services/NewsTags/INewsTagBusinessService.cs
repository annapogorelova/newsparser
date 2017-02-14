using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.BL.Services.NewsTags
{
    /// <summary>
    /// Provides a business layer functionality for NewsTag data access
    /// </summary>
    public interface INewsTagBusinessService
    {
        /// <summary>
        /// Get news tags
        /// </summary>
        /// <returns>IQueryable of NewsTag</returns>
        IQueryable<NewsTag> GetNewsTags();

        /// <summary>
        /// Get news tag by id
        /// </summary>
        /// <param name="id">News tag id</param>
        /// <returns>NewsTag object</returns>
        NewsTag GetNewsTagById(int id);

        /// <summary>
        /// Gets news tag by name
        /// </summary>
        /// <param name="name">News tag name</param>
        /// <returns>NewsTag object</returns>
        NewsTag GetNewsTagByName(string name);

        /// <summary>
        /// Inserts a news tag
        /// </summary>
        /// <param name="newsTag">NewsTag object</param>
        /// <returns>NewsTag object</returns>
        NewsTag AddNewsTag(NewsTag newsTag);

        /// <summary>
        /// Inserts a list of news tags
        /// </summary>
        /// <param name="newsTags">List of NewsTag</param>
        void AddNewsTags(List<NewsTag> newsTags);
    }
}
