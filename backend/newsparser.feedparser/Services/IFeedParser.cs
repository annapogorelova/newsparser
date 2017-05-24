using System.Collections.Generic;
using System.Threading.Tasks;
using newsparser.FeedParser.Models;
using NewsParser.DAL.Models;
using NewsParser.FeedParser.Models;

namespace NewsParser.FeedParser.Services
{
    /// <summary>
    /// Interface contains methods for feed parsing
    /// </summary>
    public interface IFeedParser
    {
        /// <summary>
        /// Parse existing RSS news source to get new news
        /// </summary>
        /// <param name="newsSource">NewsSource object</param>
        /// <returns>List of NewsItemParseModel</returns>
        Task<List<NewsItemModel>> ParseNewsSource(NewsSource newsSource);

        /// <summary>
        /// Get the basic info of RSS source (channel name, etc.)
        /// </summary>
        /// <param name="rssUrl">RSS url</param>
        /// <returns>NewsSource object</returns>
        Task<NewsSourceModel> ParseRssSource(string rssUrl);
    }
}
