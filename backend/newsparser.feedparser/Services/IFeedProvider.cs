using System.Threading.Tasks;
using System.Xml.Linq;

namespace NewsParser.FeedParser.Services
{
    public interface IFeedProvider
    {
        Task<XElement> GetFeedXml(string feedUrl);
    }
}