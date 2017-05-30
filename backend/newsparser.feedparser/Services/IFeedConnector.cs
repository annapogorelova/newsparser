using System.Collections.Generic;
using System.Threading.Tasks;
using NewsParser.DAL.Models;
using NewsParser.FeedParser.Models;

namespace NewsParser.FeedParser.Services
{
    public interface IFeedConnector
    {
        Task<FeedSource> GetFeedSource(string feedUrl);

        Task<List<FeedItem>> GetFeed(string feedUrl, FeedFormat feedFormat);
    }
}