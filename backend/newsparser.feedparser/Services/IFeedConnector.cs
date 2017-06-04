using System.Collections.Generic;
using System.Threading.Tasks;
using NewsParser.DAL.Models;
using NewsParser.FeedParser.Models;
using FeedItemModel = NewsParser.FeedParser.Models.FeedItemModel;

namespace NewsParser.FeedParser.Services
{
    public interface IFeedConnector
    {
        Task<Models.ChannelModel> GetFeedSource(string feedUrl);

        Task<List<FeedItemModel>> GetFeed(string feedUrl, FeedFormat feedFormat);
    }
}