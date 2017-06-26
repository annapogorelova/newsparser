using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using NewsParser.DAL.Models;
using NewsParser.FeedParser.Models;
using FeedItemModel = NewsParser.FeedParser.Models.FeedItemModel;

namespace NewsParser.FeedParser.Services
{
    public interface IFeedConnector
    {
        Task<Models.ChannelModel> GetFeedSource(string feedUrl);

        List<FeedItemModel> ParseFeed(XElement feedXml, FeedFormat feedFormat);

        Task<XElement> LoadFeedXml(string rssUrl);
    }
}