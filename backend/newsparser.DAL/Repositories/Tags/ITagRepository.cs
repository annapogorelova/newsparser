using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Tags
{
    /// <summary>
    /// Provides a functionality to access the Tag entity data
    /// </summary>
    public interface ITagRepository
    {
        /// <summary>
        /// Gets all tags
        /// </summary>
        /// <returns>IQueryable of Tag</returns>
        IQueryable<Tag> GetAll();

        /// <summary>
        /// Gets tag by id
        /// </summary>
        /// <param name="id">Tag id</param>
        /// <returns>Tag object</returns>
        Tag GetById(int id);

        /// <summary>
        /// Gets tag by name
        /// </summary>
        /// <param name="name">Tag name</param>
        /// <returns>Tag object</returns>
        Tag GetByName(string name);

        /// <summary>
        /// Gets the list of tags of specific Feed item
        /// </summary>
        /// <param name="feedItemId">Feed item id</param>
        /// <returns>IQueryable of Tag</returns>
        IQueryable<Tag> GetByFeedItemId(int feedItemId);

        /// <summary>
        /// Inserts a tag
        /// </summary>
        /// <param name="tag">Tag object</param>
        /// <returns>Tag object</returns>
        Tag Add(Tag tag);

        /// <summary>
        /// Inserts a list of tags
        /// </summary>
        /// <param name="tags">List of Tag</param>
        void Add(IEnumerable<Tag> tags);
    }
}
