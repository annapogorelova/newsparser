using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.NewsTags
{
    /// <summary>
    /// Provides a functionality to access the NewsTag entity data
    /// </summary>
    public interface INewsTagRepository
    {
        /// <summary>
        /// Gets all news tags
        /// </summary>
        /// <returns>IQueryable of NewsTag</returns>
        IQueryable<NewsTag> GetNewsTags();

        /// <summary>
        /// Gets news tag by id
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
        /// Gets the list of tags of specific news item
        /// </summary>
        /// <param name="newsItemId">News item id</param>
        /// <returns>IQueryable of NewsTag</returns>
        IQueryable<NewsTag> GetNewsTagsByNewsItemId(int newsItemId);

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
        void AddNewsTags(IEnumerable<NewsTag> newsTags);
    }
}
