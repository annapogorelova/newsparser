using System.Collections.Generic;
using NewsParser.DAL.Models;

namespace NewsParser.BL.Services.Tags
{
    /// <summary>
    /// Provides a business layer functionality for Tag data access
    /// </summary>
    public interface ITagDataService
    {
        /// <summary>
        /// Get tags
        /// </summary>
        /// <returns>IEnumerable of Tag</returns>
        IEnumerable<Tag> GetAll();

        /// <summary>
        /// Get tag by id
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
        /// Inserts a tag
        /// </summary>
        /// <param name="tag">Tag object</param>
        /// <returns>Tag object</returns>
        Tag Add(Tag tag);

        /// <summary>
        /// Inserts a list of tags
        /// </summary>
        /// <param name="tags">List of Tag</param>
        void Add(List<Tag> tags);
    }
}
