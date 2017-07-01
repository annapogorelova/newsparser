using System.Collections.Generic;
using System.Threading.Tasks;
using NewsParser.DAL.Models;
using NewsParser.FeedParser.Models;
using FeedItemModel = NewsParser.FeedParser.Models.FeedItemModel;

namespace NewsParser.FeedParser.Services
{
    public interface IFeedConnector
    {
        Task<ChannelModel> ParseFeedSource(string feedUrl);

        Task<List<FeedItemModel>> ParseFeed(string feedUrl, FeedFormat feedFormat);
    }
}