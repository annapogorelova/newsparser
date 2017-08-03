using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NewsParser.FeedParser.Exceptions;

namespace NewsParser.FeedParser.Services
{
    public class FeedProvider : IFeedProvider
    {
        public async Task<XElement> GetFeedXml(string feedUrl)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                var response = await httpClient.GetStringAsync(feedUrl);
                return XElement.Parse(response);
            }
            catch (Exception e)
            {
                throw new FeedParsingException($"Failed to parse RSS {feedUrl} source", e);
            }
        }
    }
}