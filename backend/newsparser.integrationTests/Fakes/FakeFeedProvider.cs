using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using NewsParser.FeedParser.Services;

namespace NewsParser.IntegrationTests.Fakes
{
    public class FakeFeedProvider : IFeedProvider
    {
        public Task<XElement> GetFeedXml(string feedUrl)
        {
            var xml = XElement.Load("TestData/TestFeed.xml");
            return Task.FromResult(xml);
        }
    }
}